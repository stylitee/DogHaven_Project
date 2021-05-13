using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.OtherPageFunctions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SellerTypeApplication : ContentPage
    {
        public SellerTypeApplication()
        {
            InitializeComponent();
            LoadTermsAndCondition();
        }

        private void LoadTermsAndCondition()
        {
            txtTItle.Text = "TERMS AND CONDITIONS OF DOG COMMERCIAL SALE" + Environment.NewLine + " ";

            txtTitle1.Text = "1. OFFER, CONFIRMATION OR AGREEMENT";

            txtDesc1.Text = "Any Offer is expressly made conditional on Buyer's assent to all of the " +
                "terms contained in the Offer without deviation. Acceptance by Buyer of an Offer may be " +
                "evidenced by (i) Buyer's written or verbal assent or the written or verbal assent of any " +
                "representative of Buyer, (ii) Buyer's acceptance of delivery of any kind of dogs or any " +
                "such acceptance by any representative of Buyer, or (iii) other conduct by Buyer or any " +
                "representative of Buyer consistent with acceptance of the Offer." + Environment.NewLine + Environment.NewLine +
                "In the event that any Offer or Confirmation is sent in response to Buyer's purchase order, " +
                "the terms and conditions of that Offer or Confirmation, including these Terms and Conditions, " +
                "shall apply to any delivery by Seller, irrespective of whether Buyer submits additional purchase" +
                " orders (electronically or otherwise) and whether Seller provides a Confirmation to such additional" +
                " purchase orders. All terms and conditions of such documents by Buyer are hereby rejected." + Environment.NewLine + Environment.NewLine +
                "If Seller receives an request of purchase from Buyer for the sale by Seller and purchase by " +
                "Buyer of Dog and such order is not a response to an Offer by Seller, or if Seller receives an " +
                "order or acceptance by Buyer which deviates from Seller's Offer, such order or acceptance, " +
                "respectively, shall be deemed to be a request for an Offer only." + Environment.NewLine + Environment.NewLine +
                "All pet dealers must possess a license; licenses must be renewed annually; licenses will not " +
                "be issued, and may be revoked, if cages and facilities do not meet proper standards of sanitation" +
                " and/or will result in inhumane treatment of animals as stated in Model Laws or Pet Shop / " +
                "Pet Dealer Model Law Section 3: Regulation (read more at: http://www.animallaw.com/Model-Law-Pet-Shops.cfm)"
                + Environment.NewLine + Environment.NewLine + "If by any chance the seller sold a defective dog within 5 days after purchase." +
                " The seller is liable to and is obliged to reimburse the buyer. This is according to Article 1568 of t" +
                "he New Civil Code which provides:" + Environment.NewLine + Environment.NewLine + "“Art. 1568. If the thing sold should be" +
                " lost in consequence of the hidden faults, and the vendor was aware of them, he shall bear the loss" +
                ", and shall be obliged to return the price and refund the expenses of the contract, with damages. I" +
                "f he was not aware of them, he shall only return the price and interest thereon, and reimburse the " +
                "expenses of the contract which the vendee might have paid.” (read more: " +
                "https://www.chanrobles.com/civilcodeofthephilippinesbook4.htm)" + Environment.NewLine + Environment.NewLine + "If an animal" +
                " should die within three (3) days after its purchase, the vendor shall be liable if the disease which ca" +
                "uses the death existed at the time of sale (Article 1578, New Civil Code of the Philippines)." + Environment.NewLine;

            txtTitle2.Text = "2. PAYMENT";
            txtDesc2.Text = "After Confirmation and Agreement of dog purchase request any transaction is done outside " +
                "of the application. The buyer and seller can use the in-app messages to start the transaction any " +
                "deliverables includes money and dog will be done outside the application on agreed conversation on " +
                "the application. ";

            txtTitle3.Text = "3. RESCHEDULING AND CANCELLATION";
            txtDesc3.Text = "No order, Agreement or any part thereof may be rescheduled or cancelled without Seller’s " +
                "prior written consent.";

            txtTitle4.Text = "4. CONFIDENTIALITY";
            txtDesc4.Text = "Except for non-confidential documentation provided to Buyer for distribution with a corre" +
                "sponding Dog Request, Buyer acknowledges that all technical, commercial and financial information (inclu" +
                "ding without limitation any source code) disclosed to Buyer by Seller is the confidential information of " +
                "Seller. Buyer shall not disclose any such confidential information to any third party and shall not use " +
                "any such confidential information for any purpose other than as agreed by the parties and in conformance w" +
                "ith the purchase transactions contemplated herein.";

            txtTitle5.Text = "5. COMPLIANCE WITH LAWS";
            txtDesc5.Text = "Each party hereto represents that it is duly authorized to enter into these Terms and Con" +
                "ditions and represents that with respect to its performance hereunder, it will comply with all applica" +
                "ble federal, state and local laws, including, but not limited to those pertaining to U.S. Export Admi" +
                "nistration or the export or import controls or restrictions of other applicable jurisdictions." + Environment.NewLine + Environment.NewLine +
                "If the delivery of Products under these Terms and Conditions is subject to the granting of an export or " +
                "import license by a government and/or any governmental authority under any applicable law or regulation, " +
                "or otherwise restricted or prohibited due to export or import control laws or regulations, Seller may susp" +
                "end its obligations and Buyer's rights regarding such delivery until such license is granted or for the d" +
                "uration of such restriction and/or prohibition, respectively, and Seller may even terminate any Agreement " +
                "related to such Products, without incurring any liability towards Buyer." + Environment.NewLine + Environment.NewLine + "Furthermore, " +
                "if an end-user statement is required, Seller shall inform Buyer immediately thereof and Buyer shall provide " +
                "Seller with such document upon Seller’s first written request; if an import license is required, Buyer shall" +
                " inform Seller immediately thereof and Buyer shall provide Seller with such document as soon as it is available."
                + Environment.NewLine + Environment.NewLine + "By accepting Seller’s Offer, entering into any Agreement and/or accepting any Products," +
                " Buyer agrees that it will not deal with the Products and/or documentation related thereto in violation of any" +
                " applicable export or import control laws and regulations.";

            txtTitle6.Text = "6. ASSIGNMENT AND SETOFF";
            txtDesc6.Text = "Buyer shall not assign any rights or obligations under these Terms and Conditions or any Agreement" +
                " without the prior written consent of Seller. Buyer hereby waives any and all rights to offset existing and fut" +
                "ure claims against any payments due for Products sold under these Terms and Conditions or under any other agreeme" +
                "nt that Buyer and Seller may have and agrees to pay the amounts hereunder regardless of any claimed offset which ma" +
                "y be asserted by Buyer or on its behalf. Seller is allowed to assign any rights or obligations under these Terms an" +
                "d Conditions and any Agreement to its affiliates or to any third party in connection with a merger or a change of control.";

            txtTitle7.Text = "GOVERNING LAW AND FORUM";
            txtDesc7.Text = "These Terms and Conditions, and all Offers, Confirmations and Agreements, are governed by and construed" +
                " in accordance with the laws of the Netherlands. All disputes arising out of or in connection with these Terms " +
                "and Conditions, or any Offer, Confirmation or Agreement, shall first be attempted by Buyer and Seller to be sett" +
                "led through consultation and negotiation in good faith and a spirit of mutual understanding. All disputes that " +
                "are not so settled within a period of thirty (30) days from the date the relevant party notified the other part" +
                "y to that effect, shall be submitted to the courts of Amsterdam, the Netherlands, provided that Seller shall al" +
                "ways be permitted to bring any action or proceedings against Buyer in any other court of competent jurisdictio" +
                "n. The United Nations Convention on Contracts for the International Sale of Goods shall not apply to these Term" +
                "s and Conditions, or any Offer, Confirmation or Agreement. Nothing is this Section 15 shall be construed or int" +
                "erpreted as a limitation on either Seller’s or Buyer’s right under applicable law for injunctive or other equita" +
                "ble relief or to take any action to safeguard its possibility to have recourse on the other party. ";

            txtTitle8.Text = "8. BREACH AND TERMINATION";
            txtDesc8.Text = "Without prejudice to any rights or remedies Seller may have under these Terms and Conditions or" +
                " the Agreement or at law, Seller may, by written notice to Buyer, terminate with immediate effect any Agree" +
                "ment, or any part thereof, without any liability whatsoever, if:" + Environment.NewLine + Environment.NewLine + "•	Buyer fails to make payment" +
                " for any Products to Seller when due;" + Environment.NewLine + "•	Buyer fails to accept conforming Products supplied hereunder;"
                + Environment.NewLine + "•	any proceedings in insolvency, bankruptcy (including reorganization) liquidation or" +
                " winding up are instituted against Buyer, whether filed or instituted by Buyer, voluntary or involuntary," +
                " a trustee or receiver is appointed over Buyer, or any assignment is made for the benefit of creditors of Buyer; or" + Environment.NewLine +
                "•	Buyer violates or breaches any of the provisions of these Terms and Conditions and/or the Agreement." + Environment.NewLine + Environment.NewLine +
                "Upon occurrence of any of the events referred to under 16(a) through 16(d) above, all payments to be made by Buyer under the Agreement shall " +
                "become immediately due and payable." + Environment.NewLine + Environment.NewLine + "In the event of cancellation, termination or expiration of" +
                " any Agreement the terms and conditions destined to survive such cancellation, termination or expiration (which shall in" +
                "clude without limitation all defined terms and Sections 4, 8 through 16 and 19 through 24 of these Terms and Conditions) " +
                "shall survive.";

            txtTitle9.Text = "9. PRODUCT AND PRODUCTION CHANGES";
            txtDesc9.Text = "Seller reserves the right to make at any time Product and/or production changes. In " +
                "such event Seller represents that said changes shall not negatively affect form, fit or function of" +
                " the Products and their performance characteristics.";

            txtTitle10.Text = "10. SEVERABILITY";
            txtDesc10.Text = "In the event that any provision(s) of the Agreement or these Terms and Conditions shall be held " +
                "invalid or unenforceable by a court of competent jurisdiction or by any future legislative or administrative " +
                "action, such holding or action shall not negate the validity or enforceability of any other provisions thereo" +
                "f.";

            txtTitle11.Text = "11. WAIVER";
            txtDesc11.Text = "The failure on the part of either party to exercise, or any delay in exercising, any right or reme" +
                "dy arising from any Offer, Confirmation or Agreement, or these Terms and Conditions, shall not operate as a waiv" +
                "er thereof; nor shall any single or partial exercise of any right or remedy arising therefrom preclude any other" +
                " or future exercise thereof or the exercise of any other right or remedy arising from any Offer, Confirmation or " +
                "Agreement, or these Terms and Conditions or by law.";

            txtTitle12.Text = "12. NOTICES";
            txtDesc12.Text = "All notices and communications to be given under these Terms and Conditions shall be in writing " +
                "and shall be deemed delivered upon hand delivery, confirmed facsimile communication, or three (3) days after d" +
                "eposit in the mail of the home country of the party, postage prepaid, by certified, registered, first class or" +
                " equivalent mail, addressed to the parties at their addresses set forth on the Offer, Confirmations and/or Agr" +
                "eement. ";

            txtTitle13.Text = "13. RELATIONSHIP OF PARTIES";
            txtDesc13.Text = "The parties hereto intend to establish a relationship of buyer and seller and as such are" +
                " independent contractors with neither party having authority as an agent or legal representative of the " +
                "other to create any obligation, express or implied, on behalf of the other.";


            txtTitle14.Text = "14. MODIFICATIONS AND CHANGES";
            txtDesc14.Text = "Seller reserves the right to make any amendments or modifications to these Terms and " +
                "Conditions at any time. Such amendments and modifications shall have effect (1) on all Offers, Confi" +
                "rmations and Agreements referring to such amended or modified Terms and Conditions as from the date" +
                " of such Offer, Confirmation or Agreement, and (2) on any existing Agreement thirty (30) days from notific" +
                "ation of such amendments or modifications by Seller to Buyer, unless Buyer has notified Seller within such t" +
                "hirty (30) days period that it objects thereto.";
        }
    }
}