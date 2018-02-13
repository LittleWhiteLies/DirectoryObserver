using DO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Should_Detect_Created_File()
        {
            string fileDirectory = @"C:/temp";
            string fileName = @"UnitTestFile.txt";

            CancellationTokenSource cancelationToken = new CancellationTokenSource();
           
            Form1 form = new Form1();

            var t = form.DoWorkAsyncInfiniteLoop(fileDirectory, "label1", cancelationToken);
            
            File.CreateText(fileDirectory + @"/" + fileName);

            t.Wait(10000);

            var result = form.Controls["label1"].Text;

            form.Close();

            Assert.AreEqual("UnitTestFile.txt Created", result);
        }

        [TestMethod]
        public void Should_Detect_Deleted_File()
        {
            string fileDirectory = @"C:/temp";
            string fileName = @"UnitTestFile.txt";

            CancellationTokenSource cancelationToken = new CancellationTokenSource();

            Form1 form = new Form1();
            
            var t = form.DoWorkAsyncInfiniteLoop(fileDirectory, "label1", cancelationToken);

            if ((File.Exists(fileDirectory + @"/" + fileName)))
            {
                File.Delete(fileDirectory + @"/" + fileName);
            }

            t.Wait(10000);
            
            var result = form.Controls["label1"].Text;

            form.Close();

            Assert.AreEqual("UnitTestFile.txt Deleted", result);
        }
    }
}
