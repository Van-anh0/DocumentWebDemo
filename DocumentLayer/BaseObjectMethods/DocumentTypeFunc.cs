using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentLayer.BaseObjectMethods
{
	public class DocumentTypeFunc
	{
		public List<DocumentType> Select_All()
		{
			using (var db = new ROUTE_MANAGEMENTEntities())
			{
				var ls = db.DocumentTypes.AsQueryable();
				if (ls != null && ls.Any())
					return ls.ToList();
				return new List<DocumentType>();
			}
		}
		public DocumentType Select_ID(int id)
		{
			using (var db = new ROUTE_MANAGEMENTEntities())
			{
				return db.DocumentTypes.FirstOrDefault(s => s.DocumentTypeID == id);
			}
		}
		public List<DocumentType> Select_IDs(List<string> IDs)
		{
			using (var db = new ROUTE_MANAGEMENTEntities())
			{
				var ls = db.DocumentTypes.AsQueryable();
				if (ls != null && ls.Any())
				{
					ls = ls.Where(s => IDs.Contains(s.DocumentTypeID.ToString()));
					return ls.ToList();
				}
				return new List<DocumentType>();
			}
		}
		public List<DocumentType> Select_By(string ColumnName, string Value)
		{
			using (var db = new ROUTE_MANAGEMENTEntities())
			{
				ColumnName = ColumnName.ToLower();
				Value = Value.ToLower();
				string sql = "Select * From DocumentType Where CONVERT(nvarchar," + ColumnName + ") = '" + Value + "'";
				var ls = db.DocumentTypes.SqlQuery(sql);
				if (ls != null && ls.Any()) return ls.ToList<DocumentType>();
				return new List<DocumentType>();
			}
		}
		public List<DocumentType> Select_By(string ColumnName, string Value, int PageSize, int PageIndex, out int TotalRows)
		{
			TotalRows = 0;
			using (var db = new ROUTE_MANAGEMENTEntities())
			{
				ColumnName = ColumnName.ToLower();
				Value = Value.ToLower();
				string sql = "Select * From DocumentType Where CONVERT(nvarchar," + ColumnName + ") = '" + Value + "'";
				var ls = db.DocumentTypes.SqlQuery(sql);
				if (ls != null && ls.Any())
				{
					TotalRows = ls.Count();
					return ls.OrderByDescending(s => s.DocumentTypeID).Skip(PageSize * PageIndex).Take(PageSize).ToList<DocumentType>();
				}
				return new List<DocumentType>();
			}
		}
		public int InsertUpdate(DocumentType obj)
		{
			using (var db = new ROUTE_MANAGEMENTEntities())
			{
				using (var db1 = new ROUTE_MANAGEMENTEntities())
				{
					var find = db.DocumentTypes.FirstOrDefault(s => s.DocumentTypeID == obj.DocumentTypeID);
					if (find != null) db1.Entry(obj).State = System.Data.Entity.EntityState.Modified;
					else obj = db1.DocumentTypes.Add(obj);
					db1.SaveChanges();
					return obj.DocumentTypeID;
				}
			}
		}
		public void Delete(int id)
		{
			using (var db = new ROUTE_MANAGEMENTEntities())
			{
				var obj = db.DocumentTypes.FirstOrDefault(s => s.DocumentTypeID == id);
				if (obj != null)
				{
					db.DocumentTypes.Remove(obj);
					db.SaveChanges();
				}
			}
		}
		public void Delete_IDs(List<string> IDs)
		{
			using (var db = new ROUTE_MANAGEMENTEntities())
			{
				var ls = db.DocumentTypes.AsQueryable();
				if (ls != null && ls.Any())
				{
					ls = ls.Where(s => IDs.Contains(s.DocumentTypeID.ToString()));
					foreach (var item in ls)
						db.DocumentTypes.Remove(item);
					db.SaveChanges();
				}
			}
		}
		public List<DocumentType> Find_KeyWord(string Keyword, int PageSize, int PageIndex, out int TotalRows)
		{
			TotalRows = 0;
			using (var db = new ROUTE_MANAGEMENTEntities())
			{
				if (!string.IsNullOrWhiteSpace(Keyword))
				{
					var obj = db.DocumentTypes.FirstOrDefault(s => s.DocumentTypeID.ToString().CompareTo(Keyword) == 0);
					if (obj != null)
					{
						List<DocumentType> ls = new List<DocumentType>();
						ls.Add(obj);
						TotalRows = 1;
						return ls;
					}
					var list = db.DocumentTypes.AsQueryable();
					list = list.Where(s => s.DocumentTypeID.ToString().Contains(Keyword)
					|| s.Name.ToLower().Contains(Keyword)

					);
					if (list != null && list.Any())
					{
						TotalRows = list.Count();
						return list.OrderByDescending(s => s.DocumentTypeID).Skip(PageSize * PageIndex).Take(PageSize).ToList();
					}
				}
				else
				{
					var list = db.DocumentTypes.AsQueryable();
					if (list != null && list.Any())
					{
						TotalRows = list.Count();
						return list.OrderByDescending(s => s.DocumentTypeID).Skip(PageSize * PageIndex).Take(PageSize).ToList();
					}
				}
				return new List<DocumentType>();
			}
		}
		public void Import(List<DocumentType> list)
		{
			using (var db = new ROUTE_MANAGEMENTEntities())
			{
				using (System.Data.Entity.DbContextTransaction transaction = db.Database.BeginTransaction())
				{
					try
					{
						db.DocumentTypes.AddRange(list);
						db.SaveChanges();
						transaction.Commit();
					}
					catch
					{
						transaction.Rollback();
					}
				}
			}
		}
	}
}
