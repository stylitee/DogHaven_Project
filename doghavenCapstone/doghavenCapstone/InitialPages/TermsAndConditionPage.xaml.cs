using doghavenCapstone.LocalDBModel;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.InitialPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermsAndConditionPage : ContentPage
    {
        public TermsAndConditionPage()
        {
            InitializeComponent();
            LoadTerms();
        }

        private void LoadTerms()
        {
            lblTerms.Text = "Doghaven Incorporated, in line with Republic Act 10173 or the Data Privacy Act " +
                "of 2012, is committed to protect and secure personal information obtained in the performan" +
                "ce of its functions. Pursuant to its mandate, Doghaven Inc. will collect your registration " +
                "form which contain your personal information." + Environment.NewLine + Environment.NewLine + Environment.NewLine +
                "In compliance with the requirements of Data Privacy Act of 2012, the Doghaven Inc. commits to " +
                "ensure that all personal information obtained will be secured and remain confidential. Collect" +
                "ed personal information will only be utilized for purposes of processing registrations and docu" +
                "mentation. The personal information shall not be shared or disclosed with other parties without" +
                " consent unless the disclosure is required by, or in compliance with applicable laws and regulat" +
                "ions. Only Doghaven Inc. app and administrator, database personnel, and other involve parties in" +
                " the company will have access to the collected personal information, which will be stored until " +
                "you delete your account or will auto delete after 2 years of inactivity for which such personal " +
                "information were originally collected. The manner of disposition of all physical documents relat" +
                "ed to the provided personal information will be based on the provision of the National Archive o" +
                "f the Philippines and/or deletion in Doghaven Inc. database. Corrections of personal information" +
                " or withdrawal of data privacy consent, if given, is done by requesting through the Doghaven appl" +
                "ication itself. ";
        }

        private void btnAccept_Clicked(object sender, EventArgs e)
        {
            TermsAndCondition term = new TermsAndCondition()
            {
                isRead = "Yes"
            };

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<TermsAndCondition>();
                conn.Insert(term);
                conn.Close();
            };
            Navigation.PushAsync(new LoginPage());
        }
    }
}