using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Aspose.Html;
using Aspose.Html.Collections;
using GenerateMedicalDocuments.AppData.DirectionToMSE.Models;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Helpers
{
    public class CreatingHTMLDocumentHelper
    {
        #region Private fields

        private StreamWriter StreamWriter;

        #endregion

        #region Construector

        public CreatingHTMLDocumentHelper(string saveFilePatch)
        {
            this.StreamWriter = new StreamWriter(saveFilePatch);
        }

        #endregion
        
        /// <summary>
        /// Генерирует и сохраняет HTML документ "Направление на медико-социальную экспертизу" по модели документа.
        /// </summary>
        /// <param name="documentModel">Модель документа.</param>
        public void CreateHTMLDocument(DirectionToMSEDocumentModel documentModel)
        {
            if (documentModel == null)
            {
                return;
            }
            
            this.GenerateHtmlTag(documentModel);
        }

        /// <summary>
        /// Создает тег "html" с содержимым.
        /// </summary>
        private void GenerateHtmlTag(DirectionToMSEDocumentModel documentModel)
        {
            this.OpenHtmlTag();
            this.GenerateHeadTag(documentModel.CreateDate);
            this.GenerateBodyTag(documentModel);
            this.CloseHtmlTag();
        }

        /// <summary>
        /// Создает тег "head" с содержимым.
        /// </summary>
        private void GenerateHeadTag(DateTime createDate)
        {
            this.StreamWriter.WriteLine("   <head>");
            this.StreamWriter.WriteLine($"      <title>Направление на медико-социальную экспертизу от {createDate.ToString("dd MMMM yyyy")}</title>");
            this.GenerateStyleTag();
            this.StreamWriter.WriteLine("   </head>");
        }

        /// <summary>
        /// Создает тег "style" с содержимым.
        /// </summary>
        private void GenerateStyleTag()
        {
            this.StreamWriter.WriteLine("       <style type=\"text/css\" media=\"screen\">");
            
            this.StreamWriter.WriteLine("");
            this.StreamWriter.WriteLine("               table.outer");
            this.StreamWriter.WriteLine("               {");
            this.StreamWriter.WriteLine("               width:100%;");
            this.StreamWriter.WriteLine("               cellspacing:0;");
            this.StreamWriter.WriteLine("               cellpadding:0;");
            this.StreamWriter.WriteLine("               border: solid 2px #999999;");
            this.StreamWriter.WriteLine("               border-collapse: collapse;");
            this.StreamWriter.WriteLine("               }");
            
            this.StreamWriter.WriteLine("");
            this.StreamWriter.WriteLine("               td.outer , th.outer");
            this.StreamWriter.WriteLine("               {");
            this.StreamWriter.WriteLine("               font: 14px Arial, Helvetica, sans-serif;");
            this.StreamWriter.WriteLine("               color: #003366;");
            this.StreamWriter.WriteLine("               border: solid 2px #999999;");
            this.StreamWriter.WriteLine("               }");
            
            this.StreamWriter.WriteLine("");
            this.StreamWriter.WriteLine("               th.outer");
            this.StreamWriter.WriteLine("               {");
            this.StreamWriter.WriteLine("               background: #EEEEEE;");
            this.StreamWriter.WriteLine("               }");
            
            this.StreamWriter.WriteLine("");
            this.StreamWriter.WriteLine("               td.outter");
            this.StreamWriter.WriteLine("               {");
            this.StreamWriter.WriteLine("               background: #fff;");
            this.StreamWriter.WriteLine("               }");
            
            this.StreamWriter.WriteLine("");
            this.StreamWriter.WriteLine("               table.Sections");
            this.StreamWriter.WriteLine("               {");
            this.StreamWriter.WriteLine("               border: none;");
            this.StreamWriter.WriteLine("               border-collapse: collapse;");
            this.StreamWriter.WriteLine("               width: 100% ;");
            this.StreamWriter.WriteLine("               margins: 0,0,0,0;");
            this.StreamWriter.WriteLine("               padding: 0,0,0,0;");
            this.StreamWriter.WriteLine("               cellspacing:0;");
            this.StreamWriter.WriteLine("               }");
            
            this.StreamWriter.WriteLine("");
            this.StreamWriter.WriteLine("               h2");
            this.StreamWriter.WriteLine("               {");
            this.StreamWriter.WriteLine("               font: bold 24px Arial, Helvetica, sans-serif;");
            this.StreamWriter.WriteLine("               color: #333333;");
            this.StreamWriter.WriteLine("               text-align: left;");
            this.StreamWriter.WriteLine("               }");
            
            this.StreamWriter.WriteLine("");
            this.StreamWriter.WriteLine("               td.SectionTitle");
            this.StreamWriter.WriteLine("               {");
            this.StreamWriter.WriteLine("               border-top: solid 4px #999999;");
            this.StreamWriter.WriteLine("               font-style: italic;");
            this.StreamWriter.WriteLine("               font-size: 12px;");
            this.StreamWriter.WriteLine("               background: #EEEEEE;");
            this.StreamWriter.WriteLine("               text-align:left;");
            this.StreamWriter.WriteLine("               }");
            
            this.StreamWriter.WriteLine("");
            this.StreamWriter.WriteLine("               td.SubSectionTitle");
            this.StreamWriter.WriteLine("               {");
            this.StreamWriter.WriteLine("               border:none;");
            this.StreamWriter.WriteLine("               width: 6% ;");
            this.StreamWriter.WriteLine("               font: bold 12px Arial, Helvetica, sans-serif;");
            this.StreamWriter.WriteLine("               text-align: right;");
            this.StreamWriter.WriteLine("               vertical-align : top ;");
            this.StreamWriter.WriteLine("               padding-right: 5px;");
            this.StreamWriter.WriteLine("               padding-top: 4px;");
            this.StreamWriter.WriteLine("               }");
            
            this.StreamWriter.WriteLine("");
            this.StreamWriter.WriteLine("               td.Rest");
            this.StreamWriter.WriteLine("               {");
            this.StreamWriter.WriteLine("               border-top: none;");
            this.StreamWriter.WriteLine("               border-right: none;");
            this.StreamWriter.WriteLine("               border-bottom: 1px solid #CCCCCC;");
            this.StreamWriter.WriteLine("               border-left: none;");
            this.StreamWriter.WriteLine("               font: 14px Arial, Helvetica, sans-serif;");
            this.StreamWriter.WriteLine("               color: #003366;");
            this.StreamWriter.WriteLine("               text-align: bottom;");
            this.StreamWriter.WriteLine("               }");
            
            this.StreamWriter.WriteLine("");
            this.StreamWriter.WriteLine("               tr");
            this.StreamWriter.WriteLine("               {");
            this.StreamWriter.WriteLine("               vertical-align:top;");
            this.StreamWriter.WriteLine("               }");
            
            this.StreamWriter.WriteLine("");
            this.StreamWriter.WriteLine("               table.inner");
            this.StreamWriter.WriteLine("               {");
            this.StreamWriter.WriteLine("               cellspacing:1;");
            this.StreamWriter.WriteLine("               cellpadding:5;");
            this.StreamWriter.WriteLine("               border: solid 2px #999999;");
            this.StreamWriter.WriteLine("               border-collapse: collapse;");
            this.StreamWriter.WriteLine("               }");
            
            this.StreamWriter.WriteLine("");
            this.StreamWriter.WriteLine("               table.inner th, table.inner td, table.inner tr");
            this.StreamWriter.WriteLine("               {");
            this.StreamWriter.WriteLine("               font: 14px Arial, Helvetica, sans-serif;");
            this.StreamWriter.WriteLine("               color: #003366;");
            this.StreamWriter.WriteLine("               border: solid 2px #999999;");
            this.StreamWriter.WriteLine("               padding: 2px 5px;");
            this.StreamWriter.WriteLine("               }");
            
            this.StreamWriter.WriteLine("");
            this.StreamWriter.WriteLine("               th.inner");
            this.StreamWriter.WriteLine("               {");
            this.StreamWriter.WriteLine("               background: #EEEEEE;");
            this.StreamWriter.WriteLine("               }");
            
            this.StreamWriter.WriteLine("");
            this.StreamWriter.WriteLine("               td.inner");
            this.StreamWriter.WriteLine("               {");
            this.StreamWriter.WriteLine("               background: #fff;");
            this.StreamWriter.WriteLine("               }");
            this.StreamWriter.WriteLine("");
            
            this.StreamWriter.WriteLine("       </style>");
        }
        
        /// <summary>
        /// Создает тег "body" с содержимым.
        /// </summary>
        private void GenerateBodyTag(DirectionToMSEDocumentModel documentModel)
        {
            this.StreamWriter.WriteLine("   <body>"); // 1 tab.
            this.GenerateMedicalOrginazationTable(documentModel.RecordTarget.PatientRole.ProviderOrganization);
            this.StreamWriter.WriteLine($"      <h2>Направление на медико-социальную экспертизу от {documentModel.CreateDate.ToString("dd MMMM yyyy")}</h2>"); // 2 tabs.
            this.GeneratePatientInfoTable(documentModel.RecordTarget.PatientRole, documentModel.Participant.ScopingOrganization.Name);
            this.StreamWriter.WriteLine("   </body>");
        }

        #region Medical organization table

        /// <summary>
        /// Создает таблицу "Медецинская организация".
        /// </summary>
        /// <param name="organizationModel">Модель организации.</param>
        private void GenerateMedicalOrginazationTable(OrganizationModel organizationModel)
        {
            if (organizationModel is null)
            {
                return;
            }
            
            this.StreamWriter.WriteLine("       <table class=\"outer\" width=\"100%\">"); // 2 tabs.
            this.StreamWriter.WriteLine("           <col width=\"40%\"/>"); // 3 tabs.
            this.StreamWriter.WriteLine("           <col width=\"60%\"/>");
            this.StreamWriter.WriteLine("           <tr class=\"outer\">");
            this.GenerateMedicalOrginazationTableHeader();
            this.GenerateMedicalOrginazationTableData(organizationModel);
            this.StreamWriter.WriteLine("           </tr>");
            this.StreamWriter.WriteLine("       </table>"); // 2 tabs.
        }

        /// <summary>
        /// Создает заголовок таблицы "Медицинская организация".
        /// </summary>
        private void GenerateMedicalOrginazationTableHeader()
        {
            this.StreamWriter.WriteLine("               <th class=\"outer\" width=\"20%\">"); // 4 tabs.
            this.StreamWriter.WriteLine("                   Медицинская организация:"); // 5 tabs.
            this.StreamWriter.WriteLine("               </th>");
        }

        /// <summary>
        /// Создает блок данных таблицы "Медицинская организация".
        /// </summary>
        /// <param name="organizationModel">Модель организации.</param>
        private void GenerateMedicalOrginazationTableData(OrganizationModel organizationModel)
        {
            string organizationAddress = $"{organizationModel.Address.PostalCode}, {organizationModel.Address.StreetAddressLine}";
            string organizationLicense = $"{organizationModel.License.Number}, {organizationModel.License.AssigningAuthorityName}";
            string contacts = GetMedicalOrganizationContactsString(organizationModel.Contacts);
            
            this.StreamWriter.WriteLine("               <td class=\"outer\">"); // 4 tabs.
            this.StreamWriter.WriteLine($"                   <strong>Название медицинской организации: </strong> {organizationModel.Name} <br/>"); // 5 tabs.
            this.StreamWriter.WriteLine($"                   <strong>Адрес: </strong> {organizationAddress} <br/>");
            this.StreamWriter.WriteLine($"                   <strong>Лицензия: </strong> {organizationLicense} <br/>");
            this.StreamWriter.WriteLine($"                   <strong>Контакты: </strong> {contacts} <br/>");
            this.StreamWriter.WriteLine("               </td>");
        }

        /// <summary>
        /// Создает строку "Контакты" для таблицы "Медицинская организация".
        /// </summary>
        /// <param name="contacts">Список контактов.</param>
        /// <returns>Строка "Контакты" для таблицы "Медицинская организация".</returns>
        private string GetMedicalOrganizationContactsString(List<TelecomModel> contacts)
        {
            if (contacts is null)
            {
                return string.Empty;
            }
            
            string resultString = string.Empty;
            string email = string.Empty;
            string mobilePhone = string.Empty;
            string fax = string.Empty;

            foreach (var contact in contacts)
            {
                if (contact.Value.Contains("http"))
                {
                    email = contact.Value;
                }
                else
                if (contact.Value.Contains("+"))
                {
                    mobilePhone = contact.Value;
                }
                else
                {
                    fax = contact.Value;
                }
            }
            
            if (!String.IsNullOrEmpty(mobilePhone))
            {
                resultString += $"Тел.(раб.): {mobilePhone}; ";
            }

            if (!String.IsNullOrEmpty(fax))
            {
                resultString += $"Факс(раб.): {fax}; ";
            }

            if (!String.IsNullOrEmpty(email))
            {
                resultString += $"{email};";
            }
            
            return resultString;
        }
        
        #endregion

        #region Patient info table

        /// <summary>
        /// Создает таблицу "Информация о пациенте" с содержимым.
        /// </summary>
        /// <param name="patientModel">Модель пациента.</param>
        /// <param name="scopingOrganizationName">Наименование страховой организации.</param>
        private void GeneratePatientInfoTable(PatientModel patientModel, string scopingOrganizationName)
        {
            if (patientModel is null)
            {
                return;
            }
            
            this.StreamWriter.WriteLine("       <table class=\"outer\" width=\"100%\">"); // 2 tabs.
            this.StreamWriter.WriteLine("           <col width=\"40%\"/>"); // 3 tabs.
            this.StreamWriter.WriteLine("           <col width=\"60%\"/>");
            
            this.GeneratePatientInfoTableRows(patientModel, scopingOrganizationName);
        }

        /// <summary>
        /// Создает блоки данных таблицы "Информация о пациенте".
        /// </summary>
        /// <param name="patientModel"></param>
        /// <param name="scopingOrganizationName">Наименование страховой организации.</param>
        private void GeneratePatientInfoTableRows(PatientModel patientModel, string scopingOrganizationName)
        {
            this.GeneratePatientInfoTableHead("Пациент:");
            this.GeneratePatientInfoTableDateName(patientModel.PatientData.Name);
            
            this.GeneratePatientInfoTableHead("Пол пациента:");
            this.GeneratePatientInfoTableDataGender(patientModel.PatientData.Gender.DisplayName);
            
            this.GeneratePatientInfoTableHead("Дата рождения (Возраст):");
            this.GeneratePatientInfoTableDataBirtday(patientModel.PatientData.BirthDate);
            
            this.GeneratePatientInfoTableHead("Идентификаторы пациента:");
            this.GeneratePatientInfoTableDataIdentity(patientModel.SNILS, patientModel.InsurancePolicy, scopingOrganizationName);
        }

        /// <summary>
        /// Создает заголовок таблицы "Информация о пациенте".
        /// </summary>
        /// <param name="caption">Текст в заголовке.</param>
        private void GeneratePatientInfoTableHead(string caption)
        {
            this.OpenPatientInfoTableRowTag();
            
            this.StreamWriter.Write("               <th class=\"outer\" width=\"20%\">"); // 4 tabs.
            this.StreamWriter.WriteLine($"{caption} </th>");
        }

        #region Generate table data

        /// <summary>
        /// Создает разметку данных имени пациента таблицы "Информация о пациенте".
        /// </summary>
        /// <param name="nameModel">Имя пациента.</param>
        private void GeneratePatientInfoTableDateName(NameModel nameModel)
        {
            if (nameModel is null)
            {
                return;
            }
            
            this.StreamWriter.WriteLine("               <td class=\"outer\">"); // 4 tabs.
            this.StreamWriter.WriteLine($"                   <b>{nameModel.Family} {nameModel.Given} {nameModel.Patronymic}</b>"); // 5 tabs.
            this.StreamWriter.WriteLine("               </td>");
            
            this.ClosePatientInfoTableRowTag();
        }

        /// <summary>
        /// Создает разметку данных пола пациента таблицы "Информация о пациенте".
        /// </summary>
        /// <param name="gender">Пол пациента.</param>
        private void GeneratePatientInfoTableDataGender(string gender)
        {
            if (String.IsNullOrWhiteSpace(gender))
            {
                return;
            }
            
            this.StreamWriter.Write("               <td class=\"outer\">"); // 4 tabs.
            this.StreamWriter.Write(gender); // 5 tabs.
            this.StreamWriter.WriteLine("</td>");
            
            this.ClosePatientInfoTableRowTag();
        }

        /// <summary>
        /// Создает разметку данных даты рождения пациента таблицы "Информация о пациенте".
        /// </summary>
        /// <param name="birthday">Дата рождения.</param>
        private void GeneratePatientInfoTableDataBirtday(DateTime birthday)
        {
            var age = GetPatientAge(birthday);
            
            this.StreamWriter.Write("               <td class=\"outer\">"); // 4 tabs.
            this.StreamWriter.Write($"{birthday.ToString("dd.MM.yyyy")} ({age})");
            this.StreamWriter.WriteLine("</td>");
            
            this.ClosePatientInfoTableRowTag();
        }

        /// <summary>
        /// Создает разметку данных идентификаторов пациента таблицы "Информация о пациенте".
        /// </summary>
        /// <param name="snils">Номер СНИЛС.</param>
        /// <param name="insurancePolicyModel">Модель полиса ОМС.</param>
        /// <param name="scopingOrganizationName">Наименование страховой организации.</param>
        private void GeneratePatientInfoTableDataIdentity(string snils, InsurancePolicyModel insurancePolicyModel, string scopingOrganizationName)
        {
            if (String.IsNullOrWhiteSpace(snils) || insurancePolicyModel is null)
            {
                return;
            }
            
            this.StreamWriter.WriteLine("               <td class=\"outer\">"); // 4 tabs.
            this.StreamWriter.WriteLine($"                  <strong>СНИЛС: </strong> {snils} <br/>"); // 5 tabs.
            this.StreamWriter.WriteLine("                   <strong>Полис ОМС: </strong><br/>");
            this.StreamWriter.WriteLine($"                  <strong>(Серия) </strong> {insurancePolicyModel.Series} <strong>" +
                                        $"(Номер) </strong> {insurancePolicyModel.Number} ({scopingOrganizationName})");
            this.StreamWriter.WriteLine("               </td>");
            
            this.ClosePatientInfoTableRowTag();
        }
        
        #endregion

        /// <summary>
        /// Получить возраст пациента.
        /// </summary>
        /// <param name="birthday">Дата рождения.</param>
        /// <returns>Возраст пациента.</returns>
        private string GetPatientAge(DateTime birthday)
        {
            var age = DateTime.Now.Year - birthday.Year;
            if (DateTime.Now.DayOfYear < birthday.DayOfYear) age++;

            return $"{age} {this.GetAgeDeclination(age)}";
        }

        /// <summary>
        /// Получить склонение возраста.
        /// </summary>
        /// <param name="num">Возраст.</param>
        /// <returns>Склонение возраста.</returns>
        private string GetAgeDeclination(int age)
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
        /// Открывает тег строки в таблице "Информация о пациенте".
        /// </summary>
        private void OpenPatientInfoTableRowTag()
        {
            this.StreamWriter.WriteLine("           <tr class=\"outer\">"); // 3 tabs.
        }

        /// <summary>
        /// Закрывает тег строки в таблице "Информация о пациенте".
        /// </summary>
        private void ClosePatientInfoTableRowTag()
        {
            this.StreamWriter.WriteLine("           </tr>");
        }
        
        #endregion
        
        /// <summary>
        /// Начало документа.
        /// Открывает тег "html".
        /// </summary>
        private void OpenHtmlTag()
        {
            this.StreamWriter.WriteLine("<html xmlns:n1=\"urn:hl7-org:v3\" " +
                                        "xmlns:n2=\"urn:hl7-org:v3/meta/voc\" " +
                                        "xmlns:voc=\"urn:hl7-org:v3/voc\" " +
                                        "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                                        "xmlns:identity=\"urn:hl7-ru:identity\" " +
                                        "xmlns:address=\"urn:hl7-ru:address\">");
        }

        /// <summary>
        /// Конец документа.
        /// Закрываем тег "html".
        /// </summary>
        private void CloseHtmlTag()
        {
            this.StreamWriter.WriteLine("</html>");
            this.StreamWriter.Close();
        }
    }
}