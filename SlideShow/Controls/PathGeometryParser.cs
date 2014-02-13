using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace Vertigo.SlideShow.Controls
{
    /// <summary>
    /// Represents the PathGeometryParser
    /// </summary>
    /// <remarks>
    /// Recreated from the WPF AbbreviatedGeometryParser. Great help with a Silverlight version
    /// from the StringToPathGeometry Converter on CodePlex.
    /// </remarks>
	public class PathGeometryParser
	{
		private const bool AllowSign = true;
		private const bool AllowComma = true;
		private const bool IsFilled = true;
		private const bool IsClosed = true;

		private IFormatProvider _formatProvider;
		private PathFigure _figure;
		private string _pathString;
		private bool _figureStarted;
		private int _pathLength;
		private int _curIndex;
		private char _token;
		private Point _lastStart;
		private Point _lastPoint;
		private Point _secondLastPoint;

        /// <summary>
        /// Parses the specified abbreviated path geometry.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The parsed PathGeometry</returns>
		public static PathGeometry Parse(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}

			if (path.Length == 0)
			{
				throw new ArgumentException("Path string cannot be empty!", "path");
			}

			PathGeometryParser parser = new PathGeometryParser();
			return parser.ParseInternal(path);
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="PathGeometryParser"/> class.
        /// </summary>
		public PathGeometryParser()
		{
		}

        /// <summary>
        /// Parses the specified abbreviated path geometry.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The parsed PathGeometry</returns>
		private PathGeometry ParseInternal(string path)
		{
			PathGeometry pathGeometry = new PathGeometry();
			bool first = false;
			char lastCmd = ' ';

			this._formatProvider = CultureInfo.InvariantCulture;
			this._pathString = path;
			this._pathLength = path.Length;
			this._curIndex = 0;
			this._secondLastPoint = new Point(0.0, 0.0);
			this._lastPoint = new Point(0.0, 0.0);
			this._lastStart = new Point(0.0, 0.0);
			this._figureStarted = false;

			while (ReadToken())
			{
				char cmd = this._token;

				if (first)
				{
					if (cmd != 'M' && cmd != 'm' && cmd != 'f' && cmd != 'F')   
					{
						ThrowBadToken();
					}

					first = false;
				}

				switch (cmd)
				{
                    case 'f':
                    case 'F':   // fill rule
                        double fill = ReadNumber(!AllowComma);
                        pathGeometry.FillRule = (fill == 0.0 ? FillRule.EvenOdd : FillRule.Nonzero);
                        break;
					case 'm':
					case 'M':   // move to the start point of a figure
						this._lastPoint = ReadPoint(cmd, !AllowComma);
						this._figure = new PathFigure();
						this._figure.StartPoint = this._lastPoint;
						this._figure.IsFilled = IsFilled;
						this._figure.IsClosed = !IsClosed;
						this._figureStarted = true;
						this._lastStart = this._lastPoint;
						lastCmd = 'M';

						while (IsNumber(AllowComma))
						{
							this._lastPoint = ReadPoint(cmd, !AllowComma);

                            LineSegment segment = new LineSegment
                            {
                                Point = this._lastPoint
                            };
							
							this._figure.Segments.Add(segment);
							lastCmd = 'L';
						}

						break;
					case 'l':
					case 'L':   // line
					case 'h':
					case 'H':   // horizontal line
					case 'v':
					case 'V':   // vertical line
						EnsureFigure();

						do
						{
							switch (cmd)
							{
								case 'l':
								case 'L':
									this._lastPoint = ReadPoint(cmd, !AllowComma);
									break;
								case 'h':
									this._lastPoint.X += ReadNumber(!AllowComma);
									break;
								case 'H':
									this._lastPoint.X = ReadNumber(!AllowComma);
									break;
								case 'v':
									this._lastPoint.Y += ReadNumber(!AllowComma);
									break;
								case 'V':
									this._lastPoint.Y = ReadNumber(!AllowComma);
									break;
							}

                            LineSegment segment = new LineSegment
                            {
                                Point = this._lastPoint
                            };
							
							this._figure.Segments.Add(segment);
						}
						while (IsNumber(AllowComma));

						lastCmd = 'L';
						break;
					case 'c':
					case 'C':   // cubic bezier
					case 's':
					case 'S':   // smooth cubic bezier
						EnsureFigure();

						do
						{
							Point p;

							if (cmd == 's' || cmd == 'S')
							{
								if (lastCmd == 'C')
								{
									p = Reflect();
								}
								else
								{
									p = this._lastPoint;
								}

								this._secondLastPoint = ReadPoint(cmd, !AllowComma);
							}
							else
							{
								p = ReadPoint(cmd, !AllowComma);
								this._secondLastPoint = ReadPoint(cmd, AllowComma);
							}

							this._lastPoint = ReadPoint(cmd, AllowComma);

                            BezierSegment bezier = new BezierSegment
                            {
                                Point1 = p,
                                Point2 = this._secondLastPoint,
                                Point3 = this._lastPoint
                            };
							
							this._figure.Segments.Add(bezier);
							lastCmd = 'C';
						}
						while (IsNumber(AllowComma));
						break;
					case 'q':
					case 'Q':   // quadratic bezier
					case 't':
					case 'T':   // smooth quadratic bezier
						EnsureFigure();

						do
						{
							if (cmd == 't' || cmd == 'T')
							{
								if (lastCmd == 'Q')
								{
									this._secondLastPoint = Reflect();
								}
								else
								{
									this._secondLastPoint = this._lastPoint;
								}

								this._lastPoint = ReadPoint(cmd, !AllowComma);
							}
							else
							{
								this._secondLastPoint = ReadPoint(cmd, !AllowComma);
								this._lastPoint = ReadPoint(cmd, AllowComma);
							}

                            QuadraticBezierSegment quad = new QuadraticBezierSegment
                            {
                                Point1 = this._secondLastPoint,
                                Point2 = this._lastPoint
                            };
							
							this._figure.Segments.Add(quad);
							lastCmd = 'Q';
						}
						while (IsNumber(AllowComma));
						break;
					case 'a':
					case 'A':   // elliptical arc
						EnsureFigure();

						do
						{
							double width = ReadNumber(!AllowComma);
							double height = ReadNumber(AllowComma);
							double rotation = ReadNumber(AllowComma);
							bool large = ReadBool();
							bool sweep = ReadBool();

							this._lastPoint = ReadPoint(cmd, AllowComma);

                            ArcSegment arc = new ArcSegment
                            {
                                Point = this._lastPoint,
                                Size = new Size(width, height),
                                RotationAngle = rotation,
                                IsLargeArc = large,
                                SweepDirection = (sweep ? SweepDirection.Clockwise : SweepDirection.Counterclockwise)
                            };
							
							this._figure.Segments.Add(arc);
						}
						while (IsNumber(AllowComma));

						lastCmd = 'A';
						break;
					case 'z':
					case 'Z':   // close figure
						EnsureFigure();

						this._figure.IsClosed = IsClosed;
						this._figureStarted = false;
						lastCmd = 'Z';
						this._lastPoint = this._lastStart;

						pathGeometry.Figures.Add(this._figure);

						break;
					default:
						ThrowBadToken();
						break;
				}
			}

			return pathGeometry;
		}

        /// <summary>
        /// Ensures the figure is started.
        /// </summary>
		private void EnsureFigure()
		{
			if (!this._figureStarted)
			{
                this._figure = new PathFigure
                {
                    StartPoint = this._lastPoint
                };

				this._figureStarted = true;
			}
		}

        /// <summary>
        /// Reads the next number in the path.
        /// </summary>
        /// <param name="allowComma">if set to <c>true</c> [allow comma].</param>
        /// <returns>The next number in the path.</returns>
		private double ReadNumber(bool allowComma)
		{
			if (!IsNumber(allowComma))
			{
				ThrowBadToken();
			}

			bool simple = true;
			int start = this._curIndex;

			if (More() && (this._pathString[this._curIndex] == '-' || this._pathString[this._curIndex] == '+'))
			{
				this._curIndex++;
			}

			if (More() && this._pathString[this._curIndex] == 'I')
			{
				this._curIndex = Math.Min(this._curIndex + 8, this._pathLength);
				simple = false;
			}
			else if (More() && this._pathString[this._curIndex] == 'N')
			{
				this._curIndex = Math.Min(this._curIndex + 3, this._pathLength);
				simple = false;
			}
			else
			{
				SkipDigits(!AllowSign);

				if (More() && this._pathString[this._curIndex] == '.')
				{
					simple = false;
					this._curIndex++;
					SkipDigits(!AllowSign);
				}

				if (More() && (this._pathString[this._curIndex] == 'E' || this._pathString[this._curIndex] == 'e'))
				{
					simple = false;
					this._curIndex++;
					SkipDigits(AllowSign);
				}
			}

			if (simple && this._curIndex <= (start + 8))
			{
				int sign = 1;

				if (this._pathString[start] == '+')
				{
					start++;
				}
				else if (this._pathString[start] == '-')
				{
					start++;
					sign = -1;
				}

				int value = 0;

				while (start < this._curIndex)
				{
					value = value * 10 + (this._pathString[start] - '0');
					start++;
				}

				return value * sign;
			}
			else
			{
				string s = this._pathString.Substring(start, this._curIndex - start);

				try
				{
					return Convert.ToDouble(s, this._formatProvider);
				}
				catch (FormatException e)
				{
					throw new FormatException(string.Format(CultureInfo.InvariantCulture, "Unexpected character in path '{0}' at position {1}", this._pathString, this._curIndex - 1), e); 
				}
			}
		}

        /// <summary>
        /// Reads the next point from the path.
        /// </summary>
        /// <param name="cmd">The command.</param>
        /// <param name="allowComma">if set to <c>true</c> [allow comma].</param>
        /// <returns>The next point in the path.</returns>
		private Point ReadPoint(char cmd, bool allowComma)
		{
			double x = ReadNumber(allowComma);
			double y = ReadNumber(AllowComma);

			if (cmd >= 'a')
			{
				x += this._lastPoint.X;
				y += this._lastPoint.Y;
			}

			return new Point(x, y);
		}

        /// <summary>
        /// Reads the next bool (0 or 1) in the path.
        /// </summary>
        /// <returns>The next bool in the path.</returns>
		private bool ReadBool()
		{
			SkipWhiteSpace(AllowComma);

			if (More())
			{
				this._token = this._pathString[this._curIndex++];

				if (this._token == '0')
				{
					return false;
				}
				else if (this._token == '1')
				{
					return true;
				}
			}

			ThrowBadToken();

			return false;
		}

        /// <summary>
        /// Determines whether the next token in the path is a number character.
        /// </summary>
        /// <param name="allowComma">if set to <c>true</c> [allow comma].</param>
        /// <returns>
        /// 	<c>true</c> if the next token in the path is a number character; otherwise, <c>false</c>.
        /// </returns>
		private bool IsNumber(bool allowComma)
		{
			bool foundComma = SkipWhiteSpace(allowComma);

			if (More())
			{
				this._token = this._pathString[this._curIndex];

				if ((this._token == '.' || this._token == '-' || this._token == '+' || (this._token >= '0' && this._token <= '9'))
					|| this._token == 'I' || this._token == 'N')
				{
					return true;
				}
			}

			if (foundComma)
			{
				ThrowBadToken();
			}

			return false;
		}

        /// <summary>
        /// Reads the next token in the path.
        /// </summary>
        /// <returns>The next token in the path.</returns>
		private bool ReadToken()
		{
			SkipWhiteSpace(!AllowComma);

			if (More())
			{
				this._token = this._pathString[this._curIndex++];
				return true;
			}

			return false;
		}

        /// <summary>
        /// Reflects this current point.
        /// </summary>
        /// <returns>The reflected point.</returns>
		private Point Reflect()
		{
			return new Point(
				(2.0 * this._lastPoint.X) - this._secondLastPoint.X,
				(2.0 * this._lastPoint.Y) - this._secondLastPoint.Y);
		}

        /// <summary>
        /// Skips the following digits in the path.
        /// </summary>
        /// <param name="signAllowed">if set to <c>true</c> [sign allowed].</param>
		private void SkipDigits(bool signAllowed)
		{
			if (signAllowed && More() && (this._pathString[this._curIndex] == '-' || this._pathString[this._curIndex] == '+'))
			{
				this._curIndex++;
			}

			while (More() && (this._pathString[this._curIndex] >= '0') && (this._pathString[this._curIndex] <= '9'))
			{
				this._curIndex++;
			}
		}

        /// <summary>
        /// Skips the following white space in the path.
        /// </summary>
        /// <param name="allowComma">if set to <c>true</c> [allow comma].</param>
        /// <returns>
        ///     <c>true</c> if a comma was encountered; otherwise <c>false</c>.
        /// </returns>
		private bool SkipWhiteSpace(bool allowComma)
		{
			bool foundComma = false;

			while (More())
			{
				char c = this._pathString[this._curIndex];

				switch (c)
				{
					case '\t':
					case '\n':
					case '\r':
					case ' ':
						break;
					case ',':
						if (allowComma)
						{
							foundComma = true;
							allowComma = false;
						}
						else
						{
							ThrowBadToken();
						}

						break;
					default:
						if ((c > ' ') && (c <= 'z'))
						{
							return foundComma;
						}

						if (!char.IsWhiteSpace(c))
						{
							return foundComma;
						}

						break;
				}

				this._curIndex++;
			}

			return foundComma;
		}

        /// <summary>
        /// Indicates whether additional characters exist in the path.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if additional characters exist; otherwise <c>false</c>.
        /// </returns>
		private bool More()
		{
			return this._curIndex < this._pathLength;
		}

        /// <summary>
        /// Throws an exception representing a bad token.
        /// </summary>
		private void ThrowBadToken()
		{
			throw new FormatException(string.Format(CultureInfo.InvariantCulture, "Unexpected character in path '{0}' at position {1}", this._pathString, this._curIndex - 1));
		}
	}
}