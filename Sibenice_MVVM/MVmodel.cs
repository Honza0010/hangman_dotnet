using Sibenice;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Sibenice_MVVM
{
    public class MVmodel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private Hra hra;

        public Hra Hra
        {
            get
            {
                return hra;
            }
            set
            {
                hra = value; OnPropertyChanged(nameof(Hra));
            }
        }

        private int numOfMistakes;
        public int NumOfMistakes
        {
            get
            {
                return numOfMistakes;
            }
            set
            {
                numOfMistakes = value; OnPropertyChanged(nameof(NumOfMistakes));
				OnPropertyChanged(nameof(hangman1Visibility));
				OnPropertyChanged(nameof(hangman2Visibility));
				OnPropertyChanged(nameof(hangman3Visibility));
				OnPropertyChanged(nameof(hangman4Visibility));
				OnPropertyChanged(nameof(hangman5Visibility));
				OnPropertyChanged(nameof(hangman6Visibility));
				OnPropertyChanged(nameof(hangman7Visibility));
				OnPropertyChanged(nameof(hangman8Visibility));
				OnPropertyChanged(nameof(hangman9Visibility));
				OnPropertyChanged(nameof(hangman10Visibility));
				OnPropertyChanged(nameof(hangman11Visibility));
			}
        }

        private string guessedWord;

        public string GuessedWord
        {
            get
            {
                return guessedWord;
            }
            set
            {
                guessedWord = value; OnPropertyChanged(nameof(GuessedWord));
            }
        }

        private string mistakeLabel;

        public string MistakeLabel
        {
            get
            {
                return mistakeLabel;
            }
            set
            {
                mistakeLabel = value; OnPropertyChanged(nameof(MistakeLabel));
            }
        }

        private bool enabledButtons;

        public bool EnabledButtons
        {
            get
            {
                return enabledButtons;
            }
            set
            {
                enabledButtons = value; OnPropertyChanged(nameof(EnabledButtons));
            }
        }

        private ObservableCollection<ListBoxItem> lbItems = new ObservableCollection<ListBoxItem>();

        public ObservableCollection<ListBoxItem> LBItems
        {
            get
            {
                return lbItems;
            }
            set
            {
                lbItems = value; OnPropertyChanged(nameof(LBItems));
            }
        }


        public bool hangman1Visibility => NumOfMistakes >= 1;
		public bool hangman2Visibility => NumOfMistakes >= 2;
		public bool hangman3Visibility => NumOfMistakes >= 3;
		public bool hangman4Visibility => NumOfMistakes >= 4;
		public bool hangman5Visibility => NumOfMistakes >= 5;
		public bool hangman6Visibility => NumOfMistakes >= 6;
		public bool hangman7Visibility => NumOfMistakes >= 7;
		public bool hangman8Visibility => NumOfMistakes >= 8;
		public bool hangman9Visibility => NumOfMistakes >= 9;
		public bool hangman10Visibility => NumOfMistakes >= 10;
		public bool hangman11Visibility => NumOfMistakes >= 11;



		public ICommand NewGame { get; set; }
        public ICommand Guess { get; set; }

        public ICommand Info { get; set; }


        public void _NewGame()
        { 
            LBItems = new ObservableCollection<ListBoxItem>();
			NumOfMistakes = 0;
            string help = "";
            if (hra != null)
            {
                help = hra.word;
            }
            hra = new Hra(help);
            try
            {
                if (hra.guessedWord == "")
                {
                    throw new Exception("Nepovedlo se připojit k databázi");
                }
                help = hra.guessedWord;
                GuessedWord = help;
                MistakeLabel = $"Zbývá ti {11 - NumOfMistakes} pokusů";
                EnabledButtons = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void _Info()
        {
            MessageBox.Show("Hádáš po jednotlivých písmenech slovo, dokud ho neuhodneš nebo se neoběsíš. \nV okně na místě, "
                       + "kde vidíte 3 tečky, uvidíte hvězdičky představující počet písmen hádaného slova",
               "Nápověda", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        public void _Guess()
        {
            Dialog dialog = new Dialog();
            dialog.ShowDialog();

            if (dialog.Confirmed == false)
            {
                return;
            }
            try
            {
                string letter = dialog.letterInput.Text;
                bool check = hra.check(letter);
                //myListBox.Items.Add(new ListBoxItem() { Content = letter.ToLower(), HorizontalAlignment = HorizontalAlignment.Center });
                LBItems.Add(new ListBoxItem() { Content = letter.ToLower(), HorizontalAlignment = HorizontalAlignment.Center });

                if (check == false)
                {
                    NumOfMistakes++;
                    MistakeLabel = $"Zbývá ti {11 - NumOfMistakes} chybných pokusů";
                    //DrawHangMan();
                    if (NumOfMistakes == 11)
                    {
                        MessageBox.Show("Prohrál jsi", "Konec", MessageBoxButton.OK, MessageBoxImage.Information);
                        //menuGuess.IsEnabled = false;
                        //btnGuess.IsEnabled = false;
                        //guessWord.Content = hra.word;
                        EnabledButtons = false;
                        GuessedWord = hra.word;
                    }
                }
                else
                {
                    GuessedWord = hra.guessedWord;
                    if (hra.rightGuessed() == true)
                    {
                        MessageBox.Show("Vyhrál jsi", "Konec", MessageBoxButton.OK, MessageBoxImage.Information);
                        //menuGuess.IsEnabled = false;
                        //btnGuess.IsEnabled = false;
                        EnabledButtons = false;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            dialog.Close();
        }

        public MVmodel()
        {
            NewGame = new NewGameCommand(this);
            Guess = new GuessCommand(this);
            Info = new InfoCommand(this);
            MistakeLabel = "Zbývá ti 11 chybných pokusů";
            EnabledButtons = false;
            GuessedWord = "...";
        }
           


    }
}
