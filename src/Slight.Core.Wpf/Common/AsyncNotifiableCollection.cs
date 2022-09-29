using System.Collections.Generic;
using System.Windows.Data;

namespace Slight.Core
{



    public class AsyncNotifiableCollection<Target> : NotifiableCollection<Target>
    {

        public AsyncNotifiableCollection() : base()
        {
            BindingOperations.EnableCollectionSynchronization(this, this);
        }

        public AsyncNotifiableCollection(bool autoNotifyWhenCollectionChanged = true)
            : base(autoNotifyWhenCollectionChanged)
        {
            BindingOperations.EnableCollectionSynchronization(this, this);
        }


        /// <summary>
        /// create a new  AsyncObservableCollection by an exist collection
        /// </summary>
        /// <param name="collection">a exist collection</param>
        public AsyncNotifiableCollection(IEnumerable<Target> collection, bool autoNotifyWhenCollectionChanged = true)
            : base(collection, autoNotifyWhenCollectionChanged)
        {
            BindingOperations.EnableCollectionSynchronization(this, this);
        }


        /// <summary>
        /// create a new  AsyncObservableCollection by an exist array
        /// </summary>
        /// <param name="array">a exist array</param>
        public AsyncNotifiableCollection(bool autoNotifyWhenCollectionChanged, params Target[] array)
            : base(autoNotifyWhenCollectionChanged, array)
        {
            BindingOperations.EnableCollectionSynchronization(this, this);
        }
    }
}
