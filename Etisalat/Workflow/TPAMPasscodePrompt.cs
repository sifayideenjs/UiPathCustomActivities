using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etisalat.CIT.OPS.Robotics.Remote
{
    public class TPAMPasscodePrompt : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        [EditorBrowsable]
        public InArgument<string[]> MachineNames { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [EditorBrowsable]
        public InArgument<string[]> ScreenSizes { get; set; }

        [Category("Output")]
        public OutArgument<string> Passcode { get; set; }

        [Category("Output")]
        public OutArgument<string> VirtualMachine { get; set; }

        [Category("Output")]
        public OutArgument<string> ScreenSize { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            foreach(var machineName in this.MachineNames.Get(context))
            {
                Console.WriteLine(machineName);
            }

            PasscodePromptForm passcodeForm = new PasscodePromptForm(this.MachineNames.Get(context), this.ScreenSizes.Get(context));
            if(passcodeForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.Passcode.Set(context, passcodeForm.Passcode);
                this.VirtualMachine.Set(context, passcodeForm.SelectedMachine);
                this.ScreenSize.Set(context, passcodeForm.SelectedScreenSize);
            }
            else
            {
                this.Passcode.Set(context, string.Empty);
            }
        }
    }
}
