using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MAUI_Tutorial1_TodoList.Views
{
    public partial class ChatPopup : Popup
    {
        private readonly HttpClient _httpClient;

        // Observable collection for chat messages
        public ObservableCollection<ChatMessage> ChatMessages { get; set; } = new ObservableCollection<ChatMessage>();

        public ChatPopup()
        {
            InitializeComponent();

            // Bind the collection view to the ChatMessages collection
            ChatCollection.ItemsSource = ChatMessages;

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            _httpClient = new HttpClient(handler);
        }

        private async void OnSendClicked(object sender, EventArgs e)
        {
            string userMessage = UserInput.Text?.Trim();
            if (string.IsNullOrWhiteSpace(userMessage))
                return;

            // Add the user's message to the chat history
            ChatMessages.Add(new ChatMessage { Text = userMessage, IsUser = true });
            UserInput.Text = string.Empty;

            // Add a placeholder for the bot's reply
            var botMessage = new ChatMessage { Text = "Thinking...", IsUser = false };
            ChatMessages.Add(botMessage);

            try
            {
                var payload = new { question = userMessage };
                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("https://10.0.2.2:7291/api/PetChat", content);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<ChatResponseModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    // Get the full answer
                    string fullAnswer = result?.Answer ?? "No answer found.";

                    // If you want to show ONLY what's after "</think>"
                    string marker = "</think>";
                    int index = fullAnswer.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
                    if (index != -1)
                    {
                        // Keep everything after </think>
                        fullAnswer = fullAnswer.Substring(index + marker.Length).Trim();
                    }

                    // Update the placeholder with the cleaned-up answer
                    botMessage.Text = fullAnswer;
                }
                else
                {
                    botMessage.Text = "Error: Unable to connect.";
                }
            }
            catch (Exception ex)
            {
                botMessage.Text = $"Error: {ex.Message}";
            }

        }

        private void Close_Clicked(object sender, EventArgs e)
        {
            Close();
        }

        public class ChatResponseModel
        {
            public string Answer { get; set; }
        }
    }

    // Modified ChatMessage model implementing INotifyPropertyChanged
    public class ChatMessage : INotifyPropertyChanged
    {
        private string text;

        public string Text
        {
            get => text;

            set
            {
                if (text != value)
                {
                    text = value;
                    OnPropertyChanged(nameof(Text));
                }
            }
        }

        public bool IsUser { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
