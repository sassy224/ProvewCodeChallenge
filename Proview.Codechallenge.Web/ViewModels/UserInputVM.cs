using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proview.CodeChallenge.Web.ViewModels
{
    /// <summary>
    /// The model that is used to display on the view
    /// </summary>
    public class UserInputVM
    {
        /// <summary>
        /// Id of the input
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The expression string, e.g., 15*2 + 15 
        /// </summary>
        public String Expression { get; set; }
        /// <summary>
        /// The computed value of the expression, use String because we can also store error 
        /// if the expression is incorrect.
        /// </summary>
        public String Result { get; set; }
    }
}