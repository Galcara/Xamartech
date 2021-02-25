using System;
using System.Collections.Generic;

namespace Common
{

    public class ResponseMessage
    {
        public static Response CreateErrorResponse(Exception ex)
        {
            Response response = new Response();
            response.Success = false;
            response.ExceptionMessage = ex.Message;
            response.StackTrace = ex.StackTrace;
            response.Message = "Erro no banco de dados contate o administrador";
            response.Exception = ex;
            return response;
        }

        public static Response CreateDuplicateErrorResponse()
        {
            Response response = new Response();
            response.Success = false;
            response.Message = "Registro duplicado.";
            return response;
        }

        public static Response CreateSingleErrorResponse(Exception ex)
        {
            Response response = new Response();
            response.Success = false;
            response.ExceptionMessage = ex.Message;
            response.StackTrace = ex.StackTrace;
            response.Message = "Erro no banco de dados contate o administrador";
            return response;
        }

        public static Response CreateQueryErrorResponse(Exception ex)
        {
            Response response = new Response();
            response.Success = false;
            response.ExceptionMessage = ex.Message;
            response.StackTrace = ex.StackTrace;
            response.Message = "Erro no banco de dados contate o administrador";
            return response;
        }


        public static Response CreateSuccessResponse()
        {
            Response response = new Response();
            response.Success = true;
            response.Message = "Operação realizada com sucesso.";
            return response;
        }

        public static SingleResponse<T> CreateNotFoundData<T>()
        {
            SingleResponse<T> response = new SingleResponse<T>();
            response.Success = false;
            response.Message = "Registro não encontrado.";
            return response;
        }

        public static SingleResponse<T> CreateSingleSuccessResponse<T>(T item)
        {
            SingleResponse<T> response = new SingleResponse<T>();
            response.Success = true;
            response.Message = "Operação realizada com sucesso.";
            response.Data = item;
            return response;
        }

        public static SingleResponse<T> CreateSingleErrorResponse<T>(Exception ex)
        {
            SingleResponse<T> response = new SingleResponse<T>();
            response.Exception = ex;
            response.Success = false;
            response.Message = "Operação falhou.";
            return response;
        }

        public static SingleResponse<T> CreateSingleErrorResponse<T>()
        {
            SingleResponse<T> response = new SingleResponse<T>();
            response.Success = false;
            response.Message = "Operação falhou.";
            return response;
        }

        public static QueryResponse<T> CreateQuerySuccessResponse<T>(List<T> item)
        {
            QueryResponse<T> response = new QueryResponse<T>();
            response.Success = true;
            response.Message = "Operação realizada com sucesso.";
            response.Data = item;
            return response;
        }


    }
}

