using System;
using System.Collections.Generic;
using System.Linq;
using GenerateMedicalDocuments.AppData.DirectionToMSE.Models;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Helpers.MainHelpers
{
    public static class MainHelper
    {
        /// <summary>
        /// Получить возраст пациента.
        /// </summary>
        /// <param name="birthday">Дата рождения.</param>
        /// <returns>Возраст пациента.</returns>
        public static string GetPatientAge(DateTime birthday)
        {
            var age = DateTime.Now.Year - birthday.Year;
            if (DateTime.Now.DayOfYear < birthday.DayOfYear) age++;

            return $"{age} {MainHelper.GetAgeDeclination(age)}";
        }

        /// <summary>
        /// Получить склонение возраста.
        /// </summary>
        /// <param name="age">Возраст.</param>
        /// <returns>Склонение возраста.</returns>
        private static string GetAgeDeclination(int age)
        {
            if (age > 100)
            {
                age = age % 100;
            }
            if (age >= 0 && age <= 20)
            {
                if (age == 0)
                {
                    return "лет";
                }
                else if (age == 1)
                {
                    return "год";
                }
                else if (age >= 2 && age <= 4)
                {
                    return "года";
                }
                else if (age >= 5 && age <= 20)
                {
                    return "лет";
                }
            }
            else if (age > 20)
            {
                string str;
                str = age.ToString();
                string n = str[str.Length - 1].ToString();
                int m = Convert.ToInt32(n);
                if (m == 0)
                {
                    return "лет";
                }
                else if (m == 1)
                {
                    return "год";
                }
                else if (m >= 2 && m <= 4)
                {
                    return "года";
                }
                else
                {
                    return "лет";
                }
            }
            return null;
        }
        
        /// <summary>
        /// Получить строку "Фамилия Имя Отчество".
        /// </summary>
        /// <param name="nameModel">Модель имени.</param>
        /// <returns>Строка "Фамилия Имя Отчество".</returns>
        public static string GetFIO(NameModel nameModel)
        {
            if (nameModel is null)
            {
                return " ";
            }
            
            string patientFIO = " ";
            if (!String.IsNullOrWhiteSpace(nameModel.Family))
            {
                patientFIO += $"{nameModel.Family} ";
            }

            if (!String.IsNullOrWhiteSpace(nameModel.Given))
            {
                patientFIO += $"{nameModel.Given} ";
            }
            
            if (!String.IsNullOrWhiteSpace(nameModel.Patronymic))
            {
                patientFIO += nameModel.Patronymic;
            }

            return patientFIO;
        }
        
        /// <summary>
        /// Получить флаг по наименованию и значению параграфа.
        /// </summary>
        /// <param name="paragraphs">Параграфы секции.</param>
        /// <param name="caption">Наименование.</param>
        /// <param name="content">Значение.</param>
        /// <returns>Истина - устанавливаем флаг.</returns>
        public static bool GetFlagParameterInParagraphs(
            List<ParagraphModel> paragraphs, 
            string caption, 
            string content)
        {
            var paragraph = paragraphs.FirstOrDefault(p =>
                p.Caption == caption);
            
            return paragraph is null ? false : paragraph.Content.Contains(content);
        }
    }
}