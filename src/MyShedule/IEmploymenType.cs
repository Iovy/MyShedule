//..begin "File Description"
/*--------------------------------------------------------------------------------*
   Filename:  IEmploymenType.cs
   Tool:      objectiF, CSharpSSvr V7.1.23
 *--------------------------------------------------------------------------------*/
//..end "File Description"

using System;
using System.Collections.Generic;


namespace MyShedule
{	
	public interface IEmploymenType
	{
		//  public LessonType Type;
		
		 // public int TypeCode;
		
		//   public string Detail;
		
		List<IEmploymenType> GetBaseType();
	}
}