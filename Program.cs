using System;
using System.Collections.Generic;
using System.Linq;

namespace GeniusIdiotConsoleApp
{
    internal class Program
    {

        class Programm
        {

            static void Main(string[] args)
            {
                int countQuestionsAndAnswers = 5;
                string[] questions = GetQuestions(countQuestionsAndAnswers);
                int[] answers = GetAnswers(countQuestionsAndAnswers);
                string[] diagnoses = GetDiagnoses();
                Console.WriteLine("Привет, представься: ");
                string userName = Console.ReadLine();
                do
                {
                    int countRightAnswers = 0;
                    int[] randomNumbers = GetRandomNumbers(countQuestionsAndAnswers);
                    foreach (int number in randomNumbers)
                    {
                        Console.WriteLine("Вопрос №" + (number));

                        Console.WriteLine(questions[number - 1]);

                        int userAnswer = Convert.ToInt32(Console.ReadLine());

                        int rightAnswer = answers[number - 1];

                        if (userAnswer == rightAnswer)
                        {
                            countRightAnswers++;
                        }
                    }
                    Console.WriteLine("Количество правильных ответов: " + countRightAnswers);
                    Console.WriteLine($"{userName}, твой диагноз: " + diagnoses[countRightAnswers]);

                }
                while (WantAgain());

            }

            static string[] GetQuestions(int countQuestionsAndAnswers)
            {
                string[] questions = new string[countQuestionsAndAnswers];
                questions[0] = "Сколько будет два плюс два умноженное на два?";
                questions[1] = "Бревно нужно распилить на 10 частей. Сколько распилов нужно сделать?";
                questions[2] = "На двух руках 10 пальцев. Сколько пальцев на 5 руках?";
                questions[3] = "Укол делают каждые полчаса. Сколько нужно минут, чтобы сделать три укола?";
                questions[4] = "Пять свечей горело, две потухли. Сколько свечей осталось?";
                return questions;
            }

            static int[] GetAnswers(int countQuestionsAndAnswers)
            {
                int[] answers = new int[countQuestionsAndAnswers];
                answers[0] = 6;
                answers[1] = 9;
                answers[2] = 25;
                answers[3] = 60;
                answers[4] = 2;
                return answers;
            }

            static string[] GetDiagnoses()
            {
                string[] diagnoses = new string[6]; // были сомнения, не передать ли в метод переменную countQuestionsAndAnswers+1 ,
                                                    // но не уверена, что количество диагнозов всегда будет на 1 больше, чем вопросов

                                                    // Ответ: Всё верно!
                diagnoses[0] = "кретин";
                diagnoses[1] = "идиот";
                diagnoses[2] = "дурак";
                diagnoses[3] = "нормальный";
                diagnoses[4] = "талант";
                diagnoses[5] = "гений";
                return diagnoses;
            }

            static int[] GetRandomNumbers(int countQuestionsAndAnswers)
            {
                Random random = new Random();
                int[] randomNumbers = Enumerable.Range(1, countQuestionsAndAnswers).OrderBy(x => random.Next()).ToArray();
                return randomNumbers;

            }

            static bool WantAgain()//Название метода надо писать с большой буквы +
            {
                string answer;
                int attempts = 0;
                do
                {
                    Console.WriteLine("Ещё раз? Да/Нет"); //Здесь пользователь не знает что ему ответить: может быть Да/Нет, а может быть Y/N. Надо ему подсказать. + 
                    answer = Console.ReadLine().ToLower();
                    if (answer == "да") return true;
                    else if (answer == "нет") return false;
                    else
                    {
                        Console.Write(attempts == 2 ? "Будем считать, что нет." : "Непонятно. ");
                        attempts++;
                    }
                    //Когда пользователь уведомлён о том каким должен быть ответ,
                    //то можно не вводить переменную "doWantAgain", а просто возвращать true или false.
                    // A: исправила, + добавила проверку на ответы не по существу
                } while (answer != "да" && answer != "нет" && attempts < 3);
                return false;
            }
        }
    }
}

// тест