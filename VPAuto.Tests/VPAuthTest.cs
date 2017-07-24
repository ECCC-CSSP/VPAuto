using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using CSSPModelsDLL.Models;

namespace VPAuto.Tests
{
    /// <summary>
    /// Summary description for VPAuth
    /// </summary>
    [TestClass]
    public class VPAuthTest
    {
        public VPAuthTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void SaveResultsInDB()
        {
            VPAuto vpAuth = new VPAuto();

            StringBuilder sb = new StringBuilder();

            FileInfo fi = new FileInfo(@"C:\CSSP latest code\VPAuto\VPAuto.Tests\VPResult.txt");

            StreamReader sr = fi.OpenText();

            string FileText = sr.ReadToEnd();

            sr.Close();

            string baseAddress = vpAuth._TextBoxSaveSite.Text;

            //string baseAddress = "http://localhost:11562/en-CA/VisualPlumes/SaveVPScenarioRawResultsJSON";

            VPScenarioIDAndRawResults vpScenarioIDAndRawResults = new VPScenarioIDAndRawResults();
            vpScenarioIDAndRawResults.VPScenarioID = 1;
            vpScenarioIDAndRawResults.RawResults = FileText;

            string parsedContent = JsonConvert.SerializeObject(vpScenarioIDAndRawResults);

            //string uploadString = "1|||" + FileText;

            WebClient webClient = new WebClient();
            webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json;charset=utf-8");

            // Act
            string RetFromWebClient = webClient.UploadString(baseAddress, "POST", parsedContent);
            //string RetFromWebClient = webClient.UploadString(baseAddress, "POST", uploadString);
            
            // Assert
            Assert.AreEqual("\"\"", RetFromWebClient);

        }

        [TestMethod]
        public void CheckDB()
        {
            VPAuto vpAuth = new VPAuto();

            vpAuth.CheckDB();
        }

    }
}
