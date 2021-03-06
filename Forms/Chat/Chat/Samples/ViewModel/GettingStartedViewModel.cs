#region Copyright Syncfusion Inc. 2001-2017.
// ------------------------------------------------------------------------------------
// <copyright file = "GettingStartedViewModel.cs" company="Syncfusion.com">
// Copyright Syncfusion Inc. 2001 - 2019. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws.
// </copyright>
// ------------------------------------------------------------------------------------
#endregion
namespace SampleBrowser.SfChat
{
    using Syncfusion.XForms.Chat;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;

    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]

    /// <summary>
    /// A ViewModel for GettingStarted sample.
    /// </summary>
    public class GettingStartedViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Chat conversation message collection.
        /// </summary>
        private ObservableCollection<object> messages;

        /// <summary>
        /// Chat conversation authors collection.
        /// </summary>
        private ObservableCollection<Author> authorsCollection;

        /// <summary>
        /// current user of chat.
        /// </summary>
        private Author currentUser;

        /// <summary>
        /// currently message typing author.
        /// </summary>
        private Author currentlyTyping;

        /// <summary>
        /// message conversation set.
        /// </summary>
        private List<string> messageList;

        /// <summary>
        /// current message going to add in the message collection.
        /// </summary>
        private string currentMessageText = "";

        /// <summary>
        /// last retrieved message index from message set. 
        /// </summary>
        private int messageIndex = 1;

        /// <summary>
        /// last retrieved author index from author collection. 
        /// </summary>
        private int authorIndex = 0;

        /// <summary>
        /// Indicates the typing indicator visibility. 
        /// </summary>
        private bool showTypingIndicator;

        /// <summary>
        /// Chat typing indicator.
        /// </summary>
        private ChatTypingIndicator typingIndicator;

        /// <summary>
        /// Chat send message command.
        /// </summary>
        private ICommand sendMessageCommand;

        /// <summary>
        /// Message typing is performed in thread.
        /// </summary>
        private Task typing;

        /// <summary>
        /// Message is added in thread.
        /// </summary>
        private Task messageThread;

        /// <summary>
        /// Initializes a new instance of the <see cref="GettingStartedViewModel"/> class.
        /// </summary>
        public GettingStartedViewModel()
        {
            this.Messages = new ObservableCollection<object>();
            this.TypingIndicator = new ChatTypingIndicator();
            this.SendMessageCommand = new Command(SendMessage);
            this.InitializeAuthors();
            this.InitializeMessageList();
            this.CurrentUser = this.authorsCollection[0];
            this.SetAuthor();
            Device.BeginInvokeOnMainThread(() =>
            {
                this.Messages.Add(this.CreateMessage(messageList[0]));
            });
            this.InitializeChatRoomConversation();
        }

        /// <summary>
        /// Gets or sets the Chat typing indicator value.
        /// </summary>
        public ChatTypingIndicator TypingIndicator
        {
            get
            {
                return this.typingIndicator;
            }

            private set
            {
                this.typingIndicator = value;
                RaisePropertyChanged("TypingIndicator");
            }
        }

        /// <summary>
        /// Gets or sets the send message command.
        /// </summary>
        public ICommand SendMessageCommand
        {
            get
            {
                return sendMessageCommand;
            }

            set
            {
                sendMessageCommand = value;
                RaisePropertyChanged("SendMessageCommand");
            }
        }


        /// <summary>
        /// Gets or sets the value indicating whether the typing indicator is visible or not.
        /// </summary>
        public bool ShowTypingIndicator
        {
            get
            {
                return this.showTypingIndicator;
            }

            private set
            {
                this.showTypingIndicator = value;
                RaisePropertyChanged("ShowTypingIndicator");
            }
        }

        /// <summary>
        /// Gets or sets the group message conversation.
        /// </summary>
        public ObservableCollection<object> Messages
        {
            get
            {
                return this.messages;
            }

            set
            {
                this.messages = value;
                RaisePropertyChanged("Messages");
            }
        }

        /// <summary>
        /// Gets or sets the current author.
        /// </summary>
        public Author CurrentUser
        {
            get
            {
                return this.currentUser;
            }
            set
            {
                this.currentUser = value;
                RaisePropertyChanged("CurrentUser");
            }
        }

        /// <summary>
        /// Property changed handler.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Occurs when property is changed.
        /// </summary>
        /// <param name="propName">changed property name</param>
        public void RaisePropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        /// <summary>
        /// Initializes the message collection for group conversation.
        /// </summary>
        private void InitializeMessageList()
        {
            this.messageList = new List<string>
            {
                "Hi guys, good morning! I'm very delighted to share with you the news that our team is going to launch a new mobile application.",
                "Oh! That's great.",
                "That is good news.",
                "What kind of application is it and when are we going to launch?",
                "A kind of Emergency Broadcast App.",
                "Can you please elaborate?",
                "The app will help users broadcast emergency messages to friends and family from their phones with location data during emergency situations.",
                "Can we extend this idea and broadcast the data all the time? It will be better if we provide options to broadcast to selected people based on timing or profiles.",
                "That's a great idea. We can consider that in our requirement.",
                "Do we have a layout design for the new app?",
                 "We will have a dedicated design engineer to design the layout once the requirement is finalized.",
                 "Are we going to develop the app natively or hybrid?",
                 "We should develop this app in Xamarin, since it provides native experience and performance.",
                 "I haven't heard of Xamarin. What's Xamarin?",
                 "Xamarin.Forms is a new library that lets you build native UIs for iOS, Android, and Windows Phone from one shared C# codebase.",
                 "That's great! I will look into it.",
                "Yes, please do. It saves a lot of development time from what I've heard. We may have to dig deeper to know more.",
                 "Is this app going to be supported in wearable technology, too?",
                 "No, not yet. We are going to develop the mobile version first. Support for wearable watches can be considered for the next version, though.",
                "Are we going to recruit a new team? Otherwise, will we migrate our existing engineers?",
                 "Since our current project is going to be complete by the end of next month, we can move the required resources from there to the development of this app. I will all the details by the end of next week.",
                 "Wow! That's great.",
                "Cool. Cheers",

            };
        }

        /// <summary>
        /// Initializes the author collection.
        /// </summary>
        private void InitializeAuthors()
        {
            this.authorsCollection = new ObservableCollection<Author>();
            this.authorsCollection.Add(new Author() { Name = "Nancy", Avatar = "People_Circle16.png" });
            this.authorsCollection.Add(new Author() { Name = "Andrea", Avatar = "People_Circle2.png" });
            this.authorsCollection.Add(new Author() { Name = "Harrison", Avatar = "People_Circle14.png" });
            this.authorsCollection.Add(new Author() { Name = "Margaret", Avatar = "People_Circle7.png" });
            this.authorsCollection.Add(new Author() { Name = "Steven", Avatar = "People_Circle5.png" });
            this.authorsCollection.Add(new Author() { Name = "Michale", Avatar = "People_Circle23.png" });
        }

        /// <summary>
        /// Sets the current typing author.
        /// </summary>
        private async void SetAuthor()
        {
            this.currentlyTyping = this.authorsCollection[this.authorIndex];
            if (this.currentMessageText != this.messageList[1])
            {
                this.authorIndex++;
            }
            else
            {
                await Task.Delay(100).ConfigureAwait(true);
            }

            if (this.authorIndex >= this.authorsCollection.Count)
            {
                this.authorIndex = 0;
            }
        }

        /// <summary>
        /// Updating typing indicator based on the current typing author.
        /// </summary>
        private void StartTypingThread()
        {
            this.typing = new Task(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.AuthorStartedTyping();
                });
            });
        }

        /// <summary>
        /// Asynchronously adding message in the message collection.
        /// </summary>
        private void StartAddMessageThread()
        {
            this.messageThread = new Task(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.AddMessage();
                });
            });
        }

        /// <summary>
        /// Adding message to the collection.
        /// </summary>
        private void AddMessage()
        {
            if (this.messageIndex >= this.messageList.Count)
            {
                this.messageIndex = 0;
                this.authorIndex = 0;
                this.currentlyTyping = this.CurrentUser;
                this.ShowTypingIndicator = false;
                return;
            }

            this.currentMessageText = this.messageList[messageIndex];
            var message = this.CreateMessage(this.currentMessageText);
            if (this.currentlyTyping.Name == this.currentUser.Name)
            {
                message.Author = this.currentUser;
            }
            if (currentlyTyping.Name != "Harrison")
            {
                this.ShowTypingIndicator = false;
            }

            this.Messages.Add(message);
            this.messageIndex++;
        }


        /// <summary>
        /// Creating message to based on the given string.
        /// </summary>
        /// <param name="text">message.</param>
        /// <returns>The <see cref="TextMessage"/> created with the given string.</returns>
        private TextMessage CreateMessage(string text)
        {
            return new TextMessage()
            {
                DateTime = DateTime.Now,
                ShowAuthorName = true,
                Author = currentlyTyping,
                Text = text,
                ShowAvatar = true,
            };
        }

        /// <summary>
        /// Updates the typing indicator based on the current typing author.
        /// </summary>
        private void AuthorStartedTyping()
        {
            this.TypingIndicator.Authors.Clear();
            if (this.currentlyTyping.Name == "Harrison")
            {
                this.TypingIndicator.Authors.Add(this.authorsCollection[3]);
                this.TypingIndicator.Authors.Add(this.currentlyTyping);
                this.TypingIndicator.Text = this.authorsCollection[3].Name + " and " + this.currentlyTyping?.Name + " are typing ...";
            }
            else
            {
                this.TypingIndicator.Authors.Add(this.currentlyTyping);
                this.TypingIndicator.Text = this.currentlyTyping?.Name + " is typing ...";
            }

            this.TypingIndicator.AvatarViewType = Syncfusion.XForms.Chat.AvatarViewType.Image;
            this.ShowTypingIndicator = true;
        }

        /// <summary>
        /// Creating and adding message to the message collection recursively. 
        /// </summary>
        private void InitializeChatRoomConversation()
        {
            Task.Factory.StartNew(() =>
            {
                if (this.messageIndex >= this.messageList.Count)
                {
                    this.messageIndex = 0;
                    this.authorIndex = 0;
                }

                Task.Delay(1000).Wait();
                this.SetAuthor();
                if (this.CurrentUser != this.currentlyTyping)
                {
                    this.StartTypingThread();
                    Task.Delay(1000).Wait();
                    this.typing.Start();
                }

                this.StartAddMessageThread();
                Task.Delay(currentMessageText?.Length > 100 ? 1000 : 3000).Wait();
                this.messageThread.Start();
                this.InitializeChatRoomConversation();
            });
        }

        /// <summary>
        /// Send message command for chat control.
        /// </summary>
        /// <param name="args"><see cref="SendMessageEventArgs"/> as parameter.</param>
        private void SendMessage(object args)
        {
            (args as SendMessageEventArgs).Message.ShowAvatar = true;
        }
    }
}
