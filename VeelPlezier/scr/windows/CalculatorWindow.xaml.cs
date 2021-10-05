using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using JetBrains.Annotations;
using VeelPlezier.scr.enums;

namespace VeelPlezier.scr.windows
{
    /// <inheritdoc cref="System.Windows.Window" />
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    internal sealed partial class CalculatorWindow
    {
        private static readonly Color DarkGray = Color.FromRgb(38, 41, 42);
        private static readonly Color LightGray = Color.FromRgb(49, 50, 51);

        private static readonly Color SubmitButtonColor = Color.FromRgb(9, 135, 232);
        private static readonly Color DarkerSubmitButtonColor = Color.FromRgb(6, 108, 186);


        private static readonly Regex AnsRegex = new(@"(\(ans\) )", RegexOptions.Compiled);

        private readonly List<string> _fullSumList = new(15);


        private readonly List<string> _history = new(15);
        private string _currentItem = "0";
        private int _currentItemInHistory;

        private decimal _lastResult;

        internal CalculatorWindow([NotNull] TranslationLanguage language)
        {
            InitializeComponent();

            SetLanguageDictionary(language);
        }

        private static bool IsOperator([NotNull] string toCheck)
        {
            return toCheck.Length == 1 && IsOperator(toCheck[0]);
        }

        private static bool IsOperator(char toCheck)
        {
            return toCheck is 'x' or '/' or '-' or '+';
        }

        [NotNull]
        private string CurrentSumToString()
        {
            string sum = "";

            _fullSumList.ForEach(delegate(string sumItem)
            {
                if (sumItem.Equals("(ans)"))
                {
                    sum += sumItem + " " + _lastResult + " ";
                }
                else
                {
                    sum += sumItem + " ";
                }
            });

            sum = sum.Trim(' ');

            return sum;
        }

        private void UpdateFullSum()
        {
            FullSum.Text = CurrentSumToString();
        }

        private async void UpdateShownNumber()
        {
            await Task.Delay(2000);
            CurrentNumber.Text = _currentItem;
        }

        private void AddToCurrentItem(char numberToAdd)
        {
            if (_currentItem.Length >= 28)
            {
                CurrentNumber.Text = "Number can't be above 27 chars!";
                UpdateShownNumber();
                return;
            }

            if (IsOperator(_currentItem))
            {
                _fullSumList.Add(_currentItem);
                _currentItem = "" + numberToAdd;
                CurrentNumber.Text = _currentItem;

                UpdateFullSum();
                return;
            }

            if (numberToAdd == ',' || _currentItem.Contains(','))
            {
                if (numberToAdd == ',' && _currentItem.Contains(','))
                {
                    return;
                }
            }
            else
            {
                _currentItem = _currentItem.TrimStart('0');
            }

            _currentItem += numberToAdd;
            CurrentNumber.Text = _currentItem;
        }

        private void AddOperator(string operatorAsString)
        {
            if (!_fullSumList.Any() && _currentItem.Equals("0"))
            {
                _currentItem = "(ans)";
            }

            if (!IsOperator(_currentItem))
            {
                _fullSumList.Add(_currentItem);

                UpdateFullSum();

                _currentItem = operatorAsString;
                FullSum.Text = FullSum.Text + " " + operatorAsString;
                CurrentNumber.Text = "0";
            }
            else
            {
                _currentItem = operatorAsString;
                FullSum.Text = FullSum.Text.Substring(0, FullSum.Text.Length - 2) + " " + operatorAsString;
                CurrentNumber.Text = "0";
            }
        }


        private void PiButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsOperator(_currentItem))
            {
                _fullSumList.Add(_currentItem);
                _currentItem = " 3.1415926535897932384626";
                CurrentNumber.Text = _currentItem;

                UpdateFullSum();
            }
            else
            {
                _currentItem = "3.1415926535897932384626";
                CurrentNumber.Text = _currentItem;
            }
        }

        private void AnsButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsOperator(_currentItem))
            {
                _fullSumList.Add(_currentItem);
                _currentItem = _lastResult + "";
                CurrentNumber.Text = _currentItem;

                UpdateFullSum();
            }
            else
            {
                _currentItem = _lastResult + "";
                CurrentNumber.Text = _currentItem;
            }
        }


        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            _currentItem = "0";
            CurrentNumber.Text = _currentItem;
            FullSum.Text = "0";
        }

        private void DivideBy100Button_Click(object sender, RoutedEventArgs e)
        {
            if (_currentItem.Length == 1 && IsOperator(_currentItem[0]))
            {
                return;
            }

            decimal currentItemAsNumber = decimal.Parse(_currentItem);

            currentItemAsNumber /= 100;

            _currentItem = currentItemAsNumber + "";

            CurrentNumber.Text = _currentItem;
        }

        private void BackspaceButton_Click(object sender, RoutedEventArgs e)
        {
            switch (_currentItem.Length)
            {
                case 0:
                    return;
                case 1:
                    _currentItem = "";
                    break;
                default:
                    _currentItem = _currentItem.Remove(_currentItem.Length - 1, 1);
                    break;
            }

            CurrentNumber.Text = _currentItem;
        }

        private void OperatorOrNumberButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string content = button.Content.ToString();


                if (_history.Any() && _currentItemInHistory != +_history.Count - 1)
                {
                    if (!IsOperator(content))
                    {
                        _currentItem = "";
                        return;
                    }

                    string currentItemDisplayed = _history[_currentItemInHistory];
                    _currentItemInHistory = _history.Count - 1;

                    string[] items;

                    if (AnsRegex.IsMatch(currentItemDisplayed))
                    {
                        currentItemDisplayed = AnsRegex.Replace(currentItemDisplayed, "");
                        items = currentItemDisplayed.Split(' ');
                        _lastResult = Convert.ToDecimal(items[0]);
                        items[0] = "(ans)";
                    }
                    else
                    {
                        items = currentItemDisplayed.Split(' ');
                    }

                    _currentItem = items.Last();

                    _fullSumList.Clear();
                    _fullSumList.AddRange(items);

                    _fullSumList.Remove(_currentItem);
                }


                if (IsOperator(content))
                {
                    AddOperator(content);
                }
                else
                {
                    AddToCurrentItem(content[0]);
                }
            }
        }

        private void BelowOrAboveZero_Click(object sender, RoutedEventArgs e)
        {
            if (_currentItem[0] == '0' && _currentItem.Length == 1)
            {
                return;
            }

            if (_currentItem.Length == 0)
            {
                return;
            }

            if (_currentItem[0] == '-')
            {
                _currentItem = _currentItem.Substring(1, _currentItem.Length - 1);
                CurrentNumber.Text = _currentItem;
            }
            else
            {
                _currentItem = "-" + _currentItem;
                CurrentNumber.Text = _currentItem;
            }
        }

        private void CommaButton_Click(object sender, RoutedEventArgs e)
        {
            AddToCurrentItem(',');
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            _fullSumList.Add(_currentItem);

            string lastOperator = null;

            decimal resultOfSum = 0;

            foreach (string forEachSumItem in _fullSumList)
            {
                string sumItem;
                if (forEachSumItem.Equals("(ans)"))
                {
                    sumItem = _lastResult + "";
                }
                else
                {
                    sumItem = forEachSumItem;
                }

                if (IsOperator(sumItem))
                {
                    lastOperator = sumItem;
                }
                else if (lastOperator != null)
                {
                    try
                    {
                        decimal currentNumber = decimal.Parse(sumItem);

                        switch (lastOperator)
                        {
                            case "+":
                                resultOfSum += currentNumber;
                                break;

                            case "-":
                                resultOfSum -= currentNumber;
                                break;

                            case "/":
                                resultOfSum /= currentNumber;
                                break;

                            case "x":
                                resultOfSum *= currentNumber;
                                break;
                            default:
                                throw new InvalidProgramException("Yikes, you fucked up when programming this.");
                        }
                    }
                    catch (OverflowException)
                    {
                        CurrentNumber.Text = "Result can't be above 27 chars!";
                        UpdateShownNumber();

                        _fullSumList.Clear();
                        _currentItem = "0";
                        _lastResult = 0;
                        return;
                    }
                }
                else
                {
                    resultOfSum = decimal.Parse(sumItem);
                }
            }

            _history.Add(CurrentSumToString());

            _fullSumList.Clear();
            _currentItem = "0";
            _lastResult = resultOfSum;

            ResetHistoryCountToLatest();

            CurrentNumber.Text = resultOfSum + "";
        }

        private void SwitchColorButton(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            Color background = (button.Background as SolidColorBrush).Color;

            if (background.R == SubmitButtonColor.R &&
                background.G == SubmitButtonColor.G &&
                background.B == SubmitButtonColor.B)
            {
                button.Background = new SolidColorBrush(DarkerSubmitButtonColor);
            }
            else if (background.R == DarkerSubmitButtonColor.R &&
                     background.G == DarkerSubmitButtonColor.G &&
                     background.B == DarkerSubmitButtonColor.B)
            {
                button.Background = new SolidColorBrush(SubmitButtonColor);
            }
            else if (background.R == DarkGray.R &&
                     background.G == DarkGray.G &&
                     background.B == DarkGray.B)
            {
                button.Background = new SolidColorBrush(LightGray);
            }
            else
            {
                button.Background = new SolidColorBrush(DarkGray);
            }
        }

        private void HistoryDown_Click(object sender, RoutedEventArgs e)
        {
            if (!_history.Any())
            {
                return;
            }

            if (_currentItemInHistory != 0)
            {
                _currentItemInHistory -= 1;
            }

            FullSum.Text = _history.ElementAt(_currentItemInHistory);
        }

        private void HistoryUp_Click(object sender, RoutedEventArgs e)
        {
            if (!_history.Any())
            {
                return;
            }

            if (_currentItemInHistory != _history.Count - 1)
            {
                _currentItemInHistory += 1;
            }

            FullSum.Text = _history.ElementAt(_currentItemInHistory);
        }

        private void ResetHistoryCountToLatest()
        {
            if (_history.Any())
            {
                _currentItemInHistory = _history.Count - 1;
                FullSum.Text = _history.ElementAt(_currentItemInHistory);
            }
            else
            {
                _currentItemInHistory = 0;
                FullSum.Text = "";
            }
        }

        public void SetLanguageDictionary([NotNull] TranslationLanguage language)
        {
            ResourceDictionary dict = new ResourceDictionary
            {
                Source = language.UriToCalculatorResource
            };

            Resources.MergedDictionaries.Clear();
            Resources.MergedDictionaries.Add(dict);
        }
    }
}