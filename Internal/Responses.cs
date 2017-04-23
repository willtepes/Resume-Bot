using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Resume_Bot
{
    internal static class Responses
    {
        public const string Features =
            "* Provide Will's objective: Try 'Tell me about yourself'\n\n"
            + "* Provide information on Will's education: Try 'Where did you go to school?'\n\n"
            + "* Provide information on Will's experience: Try 'Tell me about your experience'\n\n"
            + "* Provide information on Will's skills: Try 'What programming languages do you know?'\n\n"
            + "* Contact Will via email: Try 'I would like to contact you'\n\n";

        public const string WelcomeMessage =
            "Hello\n\n"
            + "I am Will's Resume Bot. I am designed to answer questions about his resume.  \n"
            + "Currently I have following features  \n"
            + Features
            + "You can type 'Help' to get this information again";

        public const string HelpMessage =
            "I can do following   \n"
            + Features
            + "What would you like me to do?";
    }
}