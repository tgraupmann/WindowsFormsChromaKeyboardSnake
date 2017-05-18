using Corale.Colore.Core;
using Corale.Colore.Razer.Keyboard;
using Gma.System.MouseKeyHook;
using System;
using System.Windows.Forms;

namespace WindowsFormsChromaKeyboardSnake
{
    public partial class Form1 : Form
    {
        private static readonly float SNAKE_SPEED = 0.5f;
        private const string ITEM_BLACK_WIDOW = "Razer BlackWidow Chroma";
        private const string ITEM_BLADE = "Blade Chroma";

        /// <summary>
        /// Get keyboard events
        /// </summary>
        private static IKeyboardEvents _mKeyboardEvents = null;

        /// <summary>
        /// Basic 2d vector math
        /// </summary>
        private class Vector2
        {
            public static readonly Vector2 Right = new Vector2(1, 0);
            public static readonly Vector2 Zero = new Vector2();
            public float _mX = 0f;
            public float _mY = 0f;
            public Vector2()
            {
                _mX = 0;
                _mY = 0;
            }
            public Vector2(float x, float y)
            {
                _mX = x;
                _mY = y;
            }
            public static Vector2 operator +(Vector2 a, Vector2 b)
            {
                return new Vector2(a._mX + b._mX, a._mY + b._mY);
            }
            public static Vector2 operator -(Vector2 a, Vector2 b)
            {
                return new Vector2(a._mX - b._mX, a._mY - b._mY);
            }
            public static Vector2 operator -(Vector2 a)
            {
                return new Vector2(-a._mX, -a._mY);
            }
            public static Vector2 operator *(Vector2 a, float b)
            {
                return new Vector2(a._mX * b, a._mY * b);
            }
            public static Vector2 operator *(float a, Vector2 b)
            {
                return new Vector2(a * b._mX, a * b._mY);
            }
        }

        /// <summary>
        /// Data to track the snake
        /// </summary>
        private class SnakeData
        {
            public Vector2 _mPosition = Vector2.Zero;
            public Vector2 _mDirection = Vector2.Right;
            public Color _mColor = Color.White;
            public int GetColumn()
            {
                return (int)_mPosition._mX;
            }
            public int GetRow()
            {
                return (int)_mPosition._mY;
            }
        }

        /// <summary>
        /// Chroma key data
        /// </summary>
        private class KeyData
        {
            public Key _mKey;
            public Color _mColor;
            public static implicit operator KeyData(Key key)
            {
                KeyData keyData = new KeyData();
                keyData._mKey = key;
                keyData._mColor = Color.Black;
                return keyData;
            }
        }

        #region Key layout

        private static KeyData[,] _sKeys = null;

        private static readonly KeyData[,] KEYS_BLACK_WINDOW =
        {
            {
                Key.Invalid,
                Key.Escape,
                Key.Invalid,
                Key.Invalid,
                Key.F1,
                Key.F2,
                Key.F3,
                Key.F4,
                Key.F5,
                Key.F6,
                Key.F7,
                Key.F8,
                Key.F9,
                Key.F10,
                Key.F11,
                Key.F12,
                Key.PrintScreen,
                Key.Scroll,
                Key.Pause,
                Key.Invalid,
                Key.Invalid,
                Key.Invalid,
            },

            {
                Key.Macro1,
                Key.Oem1, //~
                Key.One,
                Key.Two,
                Key.Three,
                Key.Four,
                Key.Five,
                Key.Six,
                Key.Seven,
                Key.Eight,
                Key.Nine,
                Key.Zero,
                Key.Oem2, //-
                Key.Oem3, //+
                Key.Backspace,
                Key.Insert,
                Key.Home,
                Key.PageUp,
                Key.NumLock,
                Key.NumDivide,
                Key.NumMultiply,
                Key.NumSubtract,
            },

            {
                Key.Macro2,
                Key.Tab,
                Key.Q,
                Key.W,
                Key.E,
                Key.R,
                Key.T,
                Key.Y,
                Key.U,
                Key.I,
                Key.O,
                Key.P,
                Key.Oem4, //[
                Key.Oem5, //]
                Key.Oem6, //\
                Key.Delete,
                Key.End,
                Key.PageDown,
                Key.Num7,
                Key.Num8,
                Key.Num9,
                Key.NumAdd,
            },
            {
                Key.Macro3,
                Key.CapsLock,
                Key.A,
                Key.S,
                Key.D,
                Key.F,
                Key.G,
                Key.H,
                Key.J,
                Key.K,
                Key.L,
                Key.Oem7, //;
                Key.Oem8, //'
                Key.Enter,
                Key.Invalid,
                Key.Invalid,
                Key.Invalid,
                Key.Invalid,
                Key.Num4,
                Key.Num5,
                Key.Num6,
                Key.NumAdd,
            },
            {
                Key.Macro4,
                Key.LeftShift,
                Key.Z,
                Key.X,
                Key.C,
                Key.V,
                Key.B,
                Key.N,
                Key.M,
                Key.Oem9, //,
                Key.Oem10, //.
                Key.Oem11, //?
                Key.RightShift,
                Key.Invalid,
                Key.Invalid,
                Key.Invalid,
                Key.Up,
                Key.Invalid,
                Key.Num1,
                Key.Num2,
                Key.Num3,
                Key.NumEnter,
            },

            {
                Key.Macro5,
                Key.LeftControl,
                Key.LeftWindows,
                Key.LeftAlt,
                Key.Space,
                Key.Space,
                Key.Space,
                Key.Space,
                Key.Space,
                Key.Space,
                Key.RightAlt,
                Key.Function,
                Key.RightMenu,
                Key.RightControl,
                Key.Left,
                Key.Down,
                Key.Right,
                Key.Num0,
                Key.Num0,
                Key.NumDecimal,
                Key.NumEnter,
                Key.Invalid,
            },
        };

        private static readonly KeyData[,] KEYS_BLADE =
        {
            {
                Key.Escape,
                Key.Invalid,
                Key.Invalid,
                Key.F1,
                Key.F2,
                Key.F3,
                Key.F4,
                Key.F5,
                Key.F6,
                Key.F7,
                Key.F8,
                Key.F9,
                Key.F10,
                Key.F11,
                Key.F12,
                Key.PrintScreen,
                Key.Scroll,
                Key.Pause,
            },

            {
                Key.Oem1, //~
                Key.One,
                Key.Two,
                Key.Three,
                Key.Four,
                Key.Five,
                Key.Six,
                Key.Seven,
                Key.Eight,
                Key.Nine,
                Key.Zero,
                Key.Oem2, //-
                Key.Oem3, //+
                Key.Backspace,
                Key.Insert,
                Key.Home,
                Key.PageUp,
                Key.Invalid,
            },

            {
                Key.Tab,
                Key.Q,
                Key.W,
                Key.E,
                Key.R,
                Key.T,
                Key.Y,
                Key.U,
                Key.I,
                Key.O,
                Key.P,
                Key.Oem4, //[
                Key.Oem5, //]
                Key.Oem6, //\
                Key.Delete,
                Key.End,
                Key.PageDown,
                Key.Invalid,
            },
            {
                Key.CapsLock,
                Key.A,
                Key.S,
                Key.D,
                Key.F,
                Key.G,
                Key.H,
                Key.J,
                Key.K,
                Key.L,
                Key.Oem7, //;
                Key.Oem8, //'
                Key.Enter,
                Key.Invalid,
                Key.Invalid,
                Key.Invalid,
                Key.Invalid,
                Key.Invalid,
            },
            {
                Key.LeftShift,
                Key.Z,
                Key.X,
                Key.C,
                Key.V,
                Key.B,
                Key.N,
                Key.M,
                Key.Oem9, //,
                Key.Oem10, //.
                Key.Oem11, //?
                Key.RightShift,
                Key.Invalid,
                Key.Invalid,
                Key.Invalid,
                Key.Up,
                Key.Invalid,
                Key.Invalid,
            },

            {
                Key.LeftControl,
                Key.LeftWindows,
                Key.LeftAlt,
                Key.Space,
                Key.Space,
                Key.Space,
                Key.Space,
                Key.Space,
                Key.Space,
                Key.RightAlt,
                Key.Function,
                Key.RightMenu,
                Key.RightControl,
                Key.Left,
                Key.Down,
                Key.Right,
                Key.Invalid,
                Key.Invalid,
            },
        };

        #endregion

        private static SnakeData _sSnake = new SnakeData();

        private static Random _sRandom = new Random(123);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _mComboBoxLayout.Items.Clear();
            _mComboBoxLayout.Items.Add(ITEM_BLACK_WIDOW);
            _mComboBoxLayout.Items.Add(ITEM_BLADE);
            _mComboBoxLayout.SelectedIndex = 0;

            _mKeyboardEvents = Hook.GlobalEvents();
            _mKeyboardEvents.KeyDown += HandleKeyDown;

            for (int i = 0; i < _sKeys.GetLength(0); ++i)
            {
                for (int j = 0; j < _sKeys.GetLength(1); ++j)
                {
                    KeyData keyData = _sKeys[i, j];
                    SetColor(keyData._mKey, Color.Green);
                }
            }

            _sSnake._mColor = GetRandomColor();
            _sSnake._mDirection = Vector2.Right + new Vector2(0, (float)_sRandom.NextDouble());
            _sSnake._mPosition = new Vector2(1, _sSnake._mPosition._mY);

            _mTimerGame.Start();
        }

        private void ButtonQuit_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _sKeys.GetLength(0); ++i)
            {
                for (int j = 0; j < _sKeys.GetLength(1); ++j)
                {
                    KeyData keyData = _sKeys[i, j];
                    SetColor(keyData._mKey, Color.Black);
                }
            }

            Application.Exit();
        }

        private static void SetColor(Key key, Color color)
        {
            if (key != Key.Invalid)
            {
                Keyboard.Instance.SetKey(key, color);
            }
        }

        private static void HandleKeyDown(object sender, KeyEventArgs e)
        {
            Key key;
            string strKey = e.KeyCode.ToString();
            if (!Enum.TryParse<Key>(strKey, true, out key))
            {
                return; //no-opsg
            }

            SetColor(key, Color.Green);
        }

        private Color GetRandomColor()
        {
            switch (_sRandom.Next() % 6)
            {
                case 0:
                    return Color.Red;
                case 1:
                    return Color.Orange;
                case 2:
                    return Color.Yellow;
                case 3:
                    return Color.HotPink;
                case 4:
                    return Color.Blue;
                default:
                    return Color.Purple;
            }
        }

        private void UpdateSnake()
        {
            _sSnake._mPosition = _sSnake._mPosition + _sSnake._mDirection * SNAKE_SPEED;
            if (_sSnake._mPosition._mY < 0f)
            {
                _sSnake._mPosition._mY = 0f;
                _sSnake._mDirection._mY = -_sSnake._mDirection._mY;
            }
            else if (_sSnake._mPosition._mY > 6f)
            {
                _sSnake._mPosition._mY = 6f;
                _sSnake._mDirection._mY = -_sSnake._mDirection._mY;
            }

            // Right edge
            if (_sSnake.GetColumn() < _sKeys.GetLength(1))
            {
            }
            else
            {
                _sSnake._mDirection = -Vector2.Right + new Vector2(0, (float)_sRandom.NextDouble());
                _sSnake._mPosition = new Vector2(_sKeys.GetLength(1) - 1, _sSnake._mPosition._mY);
                _sSnake._mColor = GetRandomColor();
            }

            // Left edge
            if (_sSnake.GetColumn() >= 0)
            {
            }
            else
            {
                _sSnake._mDirection = Vector2.Right + new Vector2(0, (float)_sRandom.NextDouble());
                _sSnake._mPosition = new Vector2(1, _sSnake._mPosition._mY);
                _sSnake._mColor = GetRandomColor();
            }

            for (int i = 0; i < _sKeys.GetLength(0); ++i)
            {
                for (int j = 0; j < _sKeys.GetLength(1); ++j)
                {
                    KeyData keyData = _sKeys[i, j];
                    if (i == _sSnake.GetRow() &&
                        j == _sSnake.GetColumn())
                    { 
                        keyData._mColor = _sSnake._mColor;
                        SetColor(keyData._mKey, keyData._mColor);
                    }
                }
            }
        }

        private void TimerGame_Tick(object sender, EventArgs e)
        {
            UpdateSnake();
        }

        private void _mComboBoxLayout_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_mComboBoxLayout.SelectedIndex < 0)
            {
                return;
            }
            string item = (string)_mComboBoxLayout.Items[_mComboBoxLayout.SelectedIndex];
            switch (item)
            {
                case ITEM_BLACK_WIDOW:
                    _sKeys = KEYS_BLACK_WINDOW;
                    break;
                case ITEM_BLADE:
                    _sKeys = KEYS_BLADE;
                    break;
                default:
                    return;
            }
        }
    }
}
