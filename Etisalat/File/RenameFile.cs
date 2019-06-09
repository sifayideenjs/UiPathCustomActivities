using System.Activities;
using System.ComponentModel;
using System.IO;

namespace Etisalat.File
{
    public class RenameFile : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> DirectoryPath { get; set; }

        [Category("Output")]
        public OutArgument<bool> IsSucessfull { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            this.IsSucessfull.Set(context, false);
            string directoryPath = DirectoryPath.Get(context);
            DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);
            if (dirInfo.Exists)
            {
                foreach (FileInfo fileInfo in dirInfo.GetFiles())
                {
                    fileInfo.Delete();
                }

                this.IsSucessfull.Set(context, true);
            }
            else
            {
                this.IsSucessfull.Set(context, false);
            }
        }
    }
}
