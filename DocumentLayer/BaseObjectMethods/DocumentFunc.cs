using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentLayer.BaseObjectMethods
{
	public class DocumentFunc
	{
		
		public List<Document> Select_All()
		{
			using (var db = new ROUTE_MANAGEMENTEntities())
			{
				var ls = db.Documents.AsQueryable();
				if (ls != null && ls.Any())
					return ls.ToList();
				return new List<Document>();
			}
		}
		public Document Select_ID(int id)
		{
			using (var db = new ROUTE_MANAGEMENTEntities())
			{
				return db.Documents.FirstOrDefault(s => s.ID == id);
			}
		}

		public List<Document> Select_IDs(List<string> IDs)
		{
			using (var db = new ROUTE_MANAGEMENTEntities())
			{
				var ls = db.Documents.AsQueryable();
				if (ls != null && ls.Any())
				{
					ls = ls.Where(s => IDs.Contains(s.ID.ToString()));
					return ls.ToList();
				}
				return new List<Document>();
			}
		}
		public List<Document> Select_By(string ColumnName, string Value)
		{
			using (var db = new ROUTE_MANAGEMENTEntities())
			{
				ColumnName = ColumnName.ToLower();
				Value = Value.ToLower();
				string sql = "Select * From Document Where CONVERT(nvarchar," + ColumnName + ") = '" + Value + "'";
				var ls = db.Documents.SqlQuery(sql);
				if (ls != null && ls.Any()) return ls.ToList<Document>();
				return new List<Document>();
			}
		}
		public List<Document> Select_By(string ColumnName, string Value, int PageSize, int PageIndex, out int TotalRows)
		{
			TotalRows = 0;
			using (var db = new ROUTE_MANAGEMENTEntities())
			{
				ColumnName = ColumnName.ToLower();
				Value = Value.ToLower();
				string sql = "Select * From Document Where CONVERT(nvarchar," + ColumnName + ") = '" + Value + "'";
				var ls = db.Documents.SqlQuery(sql);
				if (ls != null && ls.Any())
				{
					TotalRows = ls.Count();
					return ls.OrderByDescending(s => s.ID).Skip(PageSize * PageIndex).Take(PageSize).ToList<Document>();
				}
				return new List<Document>();
			}
		}
		public int InsertUpdate(Document obj)
		{
			using (var db = new ROUTE_MANAGEMENTEntities())
			{
				using (var db1 = new ROUTE_MANAGEMENTEntities())
				{
					var find = db.Documents.FirstOrDefault(s => s.ID == obj.ID);
					if (find != null) db1.Entry(obj).State = EntityState.Modified;
					else obj = db1.Documents.Add(obj);
					db1.SaveChanges();
					return obj.ID;
				}
			}
		}
		public void Delete(int id)
		{
			using (var db = new ROUTE_MANAGEMENTEntities())
			{
				var obj = db.Documents.FirstOrDefault(s => s.ID == id);
				if (obj != null)
				{
					db.Documents.Remove(obj);
					db.SaveChanges();
				}
			}
		}
		public void Delete_IDs(List<string> IDs)
		{
			using (var db = new ROUTE_MANAGEMENTEntities())
			{
				var ls = db.Documents.AsQueryable();
				if (ls != null && ls.Any())
				{
					ls = ls.Where(s => IDs.Contains(s.ID.ToString()));
					foreach (var item in ls)
						db.Documents.Remove(item);
					db.SaveChanges();
				}
			}
		}
		public List<Document> Find_KeyWord(string Keyword, int PageSize, int PageIndex, out int TotalRows)
		{
			TotalRows = 0;
			using (var db = new ROUTE_MANAGEMENTEntities())
			{
				if (!string.IsNullOrWhiteSpace(Keyword))
				{
					var obj = db.Documents.FirstOrDefault(s => s.ID.ToString().CompareTo(Keyword) == 0);
					if (obj != null)
					{
						List<Document> ls = new List<Document>();
						ls.Add(obj);
						TotalRows = 1;
						return ls;
					}
					var list = db.Documents.AsQueryable();
					list = list.Where(s => s.ID.ToString().Contains(Keyword)
					|| s.Title.ToLower().Contains(Keyword)
					|| s.Path.ToLower().Contains(Keyword)
					|| s.DocumentTypeID.ToString().Contains(Keyword)
					|| s.Content.ToLower().Contains(Keyword)
					);
					if (list != null && list.Any())
					{
						TotalRows = list.Count();
						return list.OrderByDescending(s => s.ID).Skip(PageSize * PageIndex).Take(PageSize).ToList();
					}
				}
				else
				{
					var list = db.Documents.AsQueryable();
					if (list != null && list.Any())
					{
						TotalRows = list.Count();
						return list.OrderByDescending(s => s.ID).Skip(PageSize * PageIndex).Take(PageSize).ToList();
					}
				}
				return new List<Document>();
			}
		}
		public void Import(List<Document> list)
		{
			using (var db = new ROUTE_MANAGEMENTEntities())
			{
				using (DbContextTransaction transaction = db.Database.BeginTransaction())
				{
					try
					{
						db.Documents.AddRange(list);
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
