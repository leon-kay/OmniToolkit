using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Result
{

    /// <summary>
    ///     通用操作返回结果基础类
    /// </summary>
    public class Result
    {
        /// <summary>操作是否成功</summary>
        public bool Success { get; protected set; }

        /// <summary>错误消息（失败时）</summary>
        public string? ErrorMessage { get; protected set; }


        /// <summary>链路追踪 ID</summary>
        public string TraceId { get; protected set; }

        /// <summary>响应生成时间（本地时区）</summary>
        public DateTime Timestamp { get; protected set; }

        /// <summary>警告或次要信息</summary>
        public List<string> Warnings { get; protected set; }

        protected Result()
        {
            TraceId = Activity.Current?.TraceId.ToString() ?? Guid.NewGuid().ToString();
            Timestamp = DateTime.Now;
            Warnings = new List<string>();
        }

        /// <summary>创建成功结果</summary>
        public static Result Ok()
        {
            var result = new Result();
            result.Success = true;
            return result;
        }

        /// <summary>创建失败结果</summary>
        public static Result Fail(string errorMessage, string? errorCode = null)
        {
            var result = new Result();
            result.Success = false;
            result.ErrorMessage = errorMessage;
            return result;
        }

        /// <summary>添加警告信息</summary>
        public void AddWarning(string warning)
        {
            Warnings.Add(warning);
        }
    }


    /// <summary>
    ///     通用操作返回结果，携带数据负载
    /// </summary>
    /// <typeparam name="T">返回的数据类型</typeparam>
    public class Result<T> : Result
    {
        /// <summary>成功时的数据负载</summary>
        public T? Data { get; protected set; }

        protected Result()
        {
        }

        /// <summary>创建成功结果</summary>
        public static Result<T> Ok(T data)
        {
            var result = new Result<T>();
            result.Success = true;
            result.Data = data;
            return result;
        }

        /// <summary>创建失败结果</summary>
        public new static Result<T> Fail(string errorMessage, string? errorCode = null)
        {
            var result = new Result<T>();
            result.Success = false;
            result.ErrorMessage = errorMessage;
            return result;
        }

        /// <summary>转换自非泛型 Result，保留监控信息</summary>
        public static Result<T> FromResult(Result result)
        {
            var generic = new Result<T>();
            generic.Success = result.Success;
            generic.ErrorMessage = result.ErrorMessage;
            generic.TraceId = result.TraceId;
            generic.Timestamp = result.Timestamp;
            generic.Warnings = new List<string>(result.Warnings);
            return generic;
        }
    }
}
