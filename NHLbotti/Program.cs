﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Speech.Synthesis;

namespace NHLbotti

// A bot to readout most recent NHL scores
{
    class Program
    {
        static void Main(string[] args)
        {
            IWebDriver driver = new ChromeDriver(); //alustetaan uusi driver
            driver.Manage().Window.Maximize();
            driver.Url = "https://www.nhl.com/scores/";
            string text = driver.FindElement(By.XPath("//div[2]/section[1]/section[1]/span")).GetAttribute("@id");
            Console.WriteLine(text);
            IList<IWebElement> context = driver.FindElements(By.XPath("//*[contains(@class, '" + "nhl-scores__list-item--game" + "')]"));
            int index = 2; //alustetaan aloitusindeksi tuloslohkojen lukemiselle

            //keskeneräinen, pitää etsiä uniikki elementti jokaiselle tuloslohkolle!

            try
            {
                foreach (IWebElement element in context)
                {
                    IWebElement section = element.FindElement(By.XPath("//div[contains(@id, 'content')]/div/div[2]/div/section/div[2]/div/section/div/ul/li["+index+"]"));
                    index = index + 1;

                    string hometeam = element.FindElement(By.XPath("//div[2]/section[1]/section[1]/div/div/span[1]")).Text; //kotijoukkue
                    Console.WriteLine(hometeam);
                    string visitors = element.FindElement(By.XPath("//div[2]/section[1]/section[3]/div/div/span[1]")).Text; //vierasjoukkue
                    Console.WriteLine(visitors);
                    string hometeamgoals = element.FindElement(By.XPath("//div[2]/section[1]/section[1]/span")).Text; //kotitulos
                    Console.WriteLine(hometeamgoals);
                    string visitorsgoals = element.FindElement(By.XPath("//div[2]/section[1]/section[3]/span")).Text; //vierastulos
                    Console.WriteLine(visitorsgoals);

                    SpeechSynthesizer voice = new SpeechSynthesizer(); //alustetaan uusi ääni
                    voice.SelectVoiceByHints(VoiceGender.Female); //valitaan naisääni
                    voice.Speak("Game" + hometeam + "against" + visitors + "Goals" + hometeamgoals + " " + visitorsgoals);
                }
            }
            catch (Exception e) //kunnes pelien id:t loppuvat
            {
                SpeechSynthesizer voice = new SpeechSynthesizer(); //alustetaan ääni
                voice.SelectVoiceByHints(VoiceGender.Female); //valitaan naisääni
                voice.Speak("All scores are read");
                driver.Close();
            }
        }
    }
}
