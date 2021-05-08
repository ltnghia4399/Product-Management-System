using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Bunifu.DataProvider;

namespace Bunifu.DAO
{
    public class Material
    {
        private static Material instance;

        public static Material Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new Material();
                }

                return instance;
            }

            set => instance = value;
        }

        internal DataTable LoadMaterialItems()
        {
            string query = "call ShowMat()";

            return DataProvider.DataProvider.Instance.ExecuteQuery(query);
        }

        internal void InsertProductItem(string _materialID, string _materialName)
        {
            string query = "call SP_InsertMaterial( @id_cl , @ten_cl )";

            try
            {
                DataProvider.DataProvider.Instance.ExecuteNonQuery(query, new object[] {_materialID, _materialName});
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal bool MaterialExist(string _materialID)
        {
            string query = "select * from chatlieu where id_cl = @id_cl ";

            DataTable result = DataProvider.DataProvider.Instance.ExecuteQuery(query,new object[] {_materialID });

            return result.Rows.Count > 0;
        }

        internal void UpdateProductItemInformation(string _materialID, string _materialName)
        {
            string query = "call SP_UpdateMaterial( @mat_id , @mat_name )";

            try
            {
                DataProvider.DataProvider.Instance.ExecuteNonQuery(query, new object[] { _materialID, _materialName });
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal void DeleteMaterialItem(string _materialID)
        {
            string query = "call SP_DeleteMaterial( @mat_id )";

            try
            {
                DataProvider.DataProvider.Instance.ExecuteNonQuery(query, new object[] { _materialID });
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
