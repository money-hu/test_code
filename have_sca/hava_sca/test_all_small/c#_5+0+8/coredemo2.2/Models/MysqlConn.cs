using System;
using coredemo2._2.Models;
using MySql.Data.MySqlClient;
using System.Configuration;


namespace coredemo2._2{
    public class MysqlConn{
        protected MySqlConnection conn;
        

        public bool OpenConn(){
            conn=new MySqlConnection(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString);
            try{
               
                if(conn.State.ToString()!="Open"){
                    conn.Open();
                }
                return true;
            }catch(MySqlException ex){
                return false;
            }

        }
        
        public bool CloseConn(){
            try{
                conn.Close();
                return true;
            }catch(Exception ex){
                return false;
            }
        }

        public Userinfo selectByname(String name){
            string sql=string.Format("select * from teacher where name = '{0}'", name);
            MySqlCommand cmd = new MySqlCommand(sql,conn);
            MySqlDataReader dr = cmd.ExecuteReader(); 
            var p=new Userinfo();
            if(!dr.HasRows){
                return null;
            }
            while(dr.Read()){
                p.Name=(String)dr["name"];
                p.Password=(String)dr["passwd"];
                p.bz=(String)dr["bz"];
            }

            return p; 
        }

        public Boolean updateByname(String name,String bz){
            string sql=string.Format("update teacher set bz = '{0}' where name = '{1}'",bz,name);
            Console.Write("sql:{0}",sql);
            MySqlCommand cmd=new MySqlCommand(sql,conn);
            int result=cmd.ExecuteNonQuery();
            if(result==1){
                return true;
            }
            return false;
        }

    


        


    }


}