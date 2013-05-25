//..begin "File Description"
/*--------------------------------------------------------------------------------*
   Filename:  SheduleExamType.cs
   Tool:      objectiF, CSharpSSvr V7.1.23
 *--------------------------------------------------------------------------------*/
//..end "File Description"

using System;
using System.Collections.Generic;


namespace MyShedule
{	
	public enum ExamType
	{
		Exam,
		Tutorial
	}
	
	public class SheduleExamType
	{
		public SheduleExamType(ExamType type)
		{
		    Type = type;
		}
		
		/// <summary>
		/// По какому критерию отображать расписание
		/// </summary>
		public ExamType Type { get; set; }
		
		/// <summary>
		/// Числовое значение перечисления, используется в привязке к выпадающему списку
		/// </summary>
		public int TypeCode
		{
			get
			{
			    return (int)Type;
			}
		}
		
		/// <summary>
		/// Описание проекции
		/// </summary>
		public string Detail
		{
			get
			{
			    return Description(Type);
			}
		}
		
		public static string Description(ExamType type)
		{
		    switch (type)
		    {
		        case ExamType.Exam: return "Экзамен";
		        case ExamType.Tutorial: return "Консультация";
		        default :  return String.Empty;
		    }
		}
		
		public static List<SheduleExamType> GetBaseType()
		{
		    List<SheduleExamType> LessonTypes = new List<SheduleExamType>();
		    LessonTypes.Add(new SheduleExamType(ExamType.Exam));
		    LessonTypes.Add(new SheduleExamType(ExamType.Tutorial));
		    return LessonTypes;
		}
	}
}