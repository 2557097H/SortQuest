using NUnit.Framework;
using AltTester.AltTesterUnitySDK.Driver;
using UnityEngine;

public class MyFirstTest
{   //Important! If your test file is inside a folder that contains an .asmdef file, please make sure that the assembly definition references NUnit.
    public AltDriver altDriver;
    //Before any test it connects with the socket
    [OneTimeSetUp]
    public void SetUp()
    {
        altDriver =new AltDriver();
    }

    //At the end of the test closes the connection with the socket
    [OneTimeTearDown]
    public void TearDown()
    {
        altDriver.Stop();
    }

    [Test]
    public void TestStartGame()
    {
        altDriver.LoadScene("MainMenu");

       

     
        Assert.IsTrue(true);

    }

}