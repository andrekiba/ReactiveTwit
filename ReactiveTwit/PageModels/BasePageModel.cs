using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FreshMvvm;
using PropertyChanged;

namespace ReactiveTwit.PageModels
{
    [ImplementPropertyChanged]
    public class BasePageModel : FreshBasePageModel
    {

    }

    public static class NotifyPropertyChangedExtensions
    {
        public static IObservable<T> ToObservable<T>(this INotifyPropertyChanged source, Expression<Func<T>> propertyExpression)
        {
			var memberExpression = propertyExpression.Body as MemberExpression;
            return memberExpression == null
                ? Observable.Empty<T>()
                : Observable
                    .FromEventPattern<PropertyChangedEventArgs>(source, "PropertyChanged")
                    .Where(e => e.EventArgs.PropertyName == memberExpression.Member.Name)
                    .Select(_ => source.GetType()
                        //.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
						.GetRuntimeProperties()
                        .FirstOrDefault(info => info.Name == memberExpression.Member.Name))
                    .Where(info => info != null)
                    .Select(info => (T)info.GetValue(source));
        }
    }

    public static class PropertyChangedExtensions
    {
        public static void WhenAny<T, TProperty>(this T source, Action<string> action, params Expression<Func<T, TProperty>>[] properties) where T : INotifyPropertyChanged
        {
            var propertyNames = properties.ToDictionary(
                expression => FreshMvvm.PropertyChangedExtensions.GetPropertyInfo(expression).Name, 
                expression => expression.Compile());
            source.PropertyChanged += (sender, e) => {
                if (propertyNames.ContainsKey(e.PropertyName))
                {
                    action(e.PropertyName);
                }
            };
        }

        /// <summary>
        /// Gets property information for the specified <paramref name="property"/> expression.
        /// </summary>
        /// <typeparam name="TSource">Type of the parameter in the <paramref name="property"/> expression.</typeparam>
        /// <typeparam name="TValue">Type of the property's value.</typeparam>
        /// <param name="property">The expression from which to retrieve the property information.</param>
        /// <returns>Property information for the specified expression.</returns>
        /// <exception cref="ArgumentException">The expression is not understood.</exception>
        public static PropertyInfo GetPropertyInfo<TSource, TValue>(this Expression<Func<TSource, TValue>> property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            var body = property.Body as MemberExpression;
            if (body == null)
                throw new ArgumentException("Expression is not a property", nameof(property));

            var propertyInfo = body.Member as PropertyInfo;
            if (propertyInfo == null)
                throw new ArgumentException("Expression is not a property", nameof(property));

            return propertyInfo;
        }
    }
}
