using BlackJack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BlackJackTestCases
{
    
    
    /// <summary>
    ///This is a test class for DeckTest and is intended
    ///to contain all DeckTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DeckTest {


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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Shuffle
        ///</summary>
        [TestMethod()]
        public void ShuffleTest()
        {
            Deck target = new Deck(); // TODO: Initialize to an appropriate value
            target.Shuffle();
            int same = 0;
            foreach(Card c in target.deck)
            {
                foreach(Card c1 in target.deck)
                {
                    c1.Equals(c);
                    same++;

                    if(same == 2)
                        Assert.Fail("Deck has cards with the same value");
                }
                same = 0;
            }

            
        }

        /// <summary>
        ///A test for Build
        ///</summary>
        [TestMethod()]
        public void BuildTest()
        {
            Deck target = new Deck(); // TODO: Initialize to an appropriate value
            target.Build();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
