﻿using Acr.UserDialogs;
using doghavenCapstone.Model;
using doghavenCapstone.OtherPageFunctions;
using doghavenCapstone.PreventerPage;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.InitialPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewAccountVerify : ContentPage
    {
        public NewAccountVerify()
        {
            InitializeComponent();
            txtNameSpan.Text = App.fullName;
            setTextMessage();
            UserDialogs.Instance.HideLoading();
        }

        public void setTextMessage()
        {
            txtMessage.Text = "Before we proceed, we want to make the best user experience for you, " +
                "what you choose on your user type during the registration which is Seller, Personal User," +
                " Breeder and Institution, will vary on how the application works." + Environment.NewLine + Environment.NewLine +
                "We would want to explain first what this user type means is and how this will affect your " +
                "user experience" + Environment.NewLine + Environment.NewLine +"Since Dog haven can be used for breeding, " +
                "selling of goods, personal users like what information you need to know and institutions " +
                "like adoption center, veterinary clinics, pet shops and etc. The Dog Haven application will" +
                " adopt to those choices." + Environment.NewLine + Environment.NewLine + Environment.NewLine +"Breeding – if you " +
                "choose breeding, the application will focus you on matching for your dogs, and all the necessary " +
                "information that a breeder needs. But don’t worry! You still have access on other functionalities" +
                " as well." + Environment.NewLine + Environment.NewLine + "Selling – if you choose selling, the application will focus " +
                "more on marketplace, which will greatly usable for dog selling and other dog related transactions," +
                " you could be a seller that is unregistered in PCCI (Philippine Canine Club Inc.) or unregistered " +
                "user. By accepting the users and agreement you accepted also the rules regulations about the liabilities of using unregistered seller account. " +
                "Nevertheless, you can still use other functionalities" + Environment.NewLine + Environment.NewLine + "Personal " +
                "User – this user type was solely for user who wants to buy dogs, adopt dogs and dog related information" +
                ". This is preferable for users who doesn’t have any owned dogs yet. You can still use other functionalities" + Environment.NewLine + Environment.NewLine +
                "Institution – this user type can be used by facilities (e.g. adoption center or veterinary clinic)" +
                " this user type can receive notification if a user who uses Doghaven look at the dogs on your adoption" +
                " center, they can fill up a form and you will be notified for easy transactions. This could be used by" +
                " veterinary clinics, they can be notified if a user has an emergency about a dog, or the user want to" +
                " request something. You can still use other functionalities." + Environment.NewLine + Environment.NewLine + Environment.NewLine +
                "By reading this information I hope you understand what does the user type means, you can change your user" +
                " type before we proceed, start using the Dog Haven App!";
        }

        private void btnChangeUserType_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ChangeUserTypePage());
        }

        private void btnProceed_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UploadDogPage());
        }
    }
}