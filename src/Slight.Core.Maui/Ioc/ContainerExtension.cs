namespace Slight.Core
{
    /// <summary>
    /// Provides Types and Services registered with the Container
    /// <example>
    /// <para>
    /// Usage as markup extension:
    /// <![CDATA[
    ///   <TextBlock
    ///     Text="{Binding
    ///       Path=Foo,
    ///       Converter={slight:Container {x:Type local:MyConverter}}}" />
    /// ]]>
    /// </para>
    /// <para>
    /// Usage as XML element:
    /// <![CDATA[
    ///   <Window>
    ///     <Window.BindingContext>
    ///       <slight:Container ServiceType="{x:Type local:MyViewModel}" />
    ///     </Window.BindingContext>
    ///   </Window>
    /// ]]>
    /// </para>
    /// </example>
    /// </summary> 
    public class ContainerExtension : IMarkupExtension
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerExtension"/> class.
        /// </summary>
        public ContainerExtension()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerExtension"/> class.
        /// </summary>
        /// <param name="serviceType">The type to Resolve</param>
        public ContainerExtension(Type serviceType)
        {
            ServiceType = serviceType;
        }

        /// <summary>
        /// The type to Resolve
        /// </summary>
        public Type ServiceType { get; set; }

        /// <summary>
        /// Provide resolved object from <see cref="SlightApplication.Provider"/>
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            object serviceTypeInstance = SlightApplication.Provider?.Resolve(ServiceType);

            return serviceTypeInstance;
        }
    }
}
