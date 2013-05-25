//..begin "File Description"
/*--------------------------------------------------------------------------------*
   Filename:  SheduleSerializer.cs
   Tool:      objectiF, CSharpSSvr V7.1.23
 *--------------------------------------------------------------------------------*/
//..end "File Description"

using System;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using MyShedule.ScheduleClasses;

namespace MyShedule
{	
	public static class ScheduleSerializer
	{
		/// <summary>Сохранить расписание в файл </summary>
		/// <param name="path"> Путь к файлу</param>
		/// <param name="shedule"> Сохраняемое расписание</param>
		public static void SaveData(string path, ScheduleWeeks shedule)
		{
		    XmlWriter writer = new XmlTextWriter(path, System.Text.Encoding.UTF8);
		    XmlSerializer serializer = new XmlSerializer(typeof(ScheduleWeeks));
		    serializer.Serialize(writer, shedule);
		    writer.Close();
		}
		
		/// <summary>Прочитать расписание из файла</summary>
		/// <param name="path">Путь к файлу</param>
		/// <returns>Полученное расписание</returns>
		public static ScheduleWeeks ReadData(string path)
		{
		    XmlReader reader = new XmlTextReader(path);
		    XmlSerializer serializer = new XmlSerializer(typeof(ScheduleWeeks)); 
		    ScheduleWeeks shedule = (ScheduleWeeks)serializer.Deserialize(reader);
		    reader.Close();
		    return shedule;
		}
	}
}