// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Kona.Infrastructure.Interfaces;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Kona.Infrastructure
{
    public class FileBackedRestorableStateService : IRestorableStateService
    {
        private Dictionary<string, object> _stateBag = new Dictionary<string, object>();
        private List<Type> _knownTypes = new List<Type>();

        /// <summary>
        /// List of custom types provided to the <see cref="DataContractSerializer"/> when
        /// reading and writing state.  Initially empty, additional types may be
        /// added to customize the serialization process.
        /// </summary>
        public List<Type> KnownTypes
        {
            get { return _knownTypes; }
        }

        public void SaveState(string key, object state)
        {
            if (_stateBag != null && !string.IsNullOrWhiteSpace(key))
                _stateBag[key] = state;
        }

        public object GetState(string key)
        {
            if (_stateBag != null && _stateBag.ContainsKey(key))
                return _stateBag[key];
            return null;
        }

        /// <summary>
        /// Save the current state bag.
        /// </summary>
        /// <returns>An asynchronous task that reflects when the state bag has been saved.</returns>
        public async Task SaveAsync()
        {
            try
            {
                // Serialize the state bag synchronously to avoid asynchronous access to shared
                // state
                MemoryStream sessionData = new MemoryStream();
                DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, object>), _knownTypes);
                serializer.WriteObject(sessionData, _stateBag);

                // Get an output stream for the SessionState file and write the state asynchronously
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(Constants.RestorableStateFileName, CreationCollisionOption.ReplaceExisting);
                using (Stream fileStream = await file.OpenStreamForWriteAsync())
                {
                    sessionData.Seek(0, SeekOrigin.Begin);
                    await sessionData.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                }
            }
            catch (Exception e)
            {
                throw new RestorableStateServiceException(e);
            }
        }
        /// <summary>
        /// Restores previously saved state bag.
        /// </summary>
        /// <returns>An asynchronous task that reflects when the state bag has been read.</returns>
        public async Task RestoreAsync()
        {
            _stateBag = new Dictionary<String, Object>();

            try
            {
                // Get the input stream for the SessionState file
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(Constants.RestorableStateFileName);
                using (IInputStream inStream = await file.OpenSequentialReadAsync())
                {
                    // Deserialize the Session State
                    DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, object>), _knownTypes);
                    _stateBag = (Dictionary<string, object>)serializer.ReadObject(inStream.AsStreamForRead());
                }
            }
            catch (Exception e)
            {
                throw new RestorableStateServiceException(e);
            }
        }

        public async Task ClearAsync()
        {
            _stateBag = new Dictionary<String, Object>();

            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(Constants.SessionStateFileName);
                await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
            catch (FileNotFoundException){}
        }
    }
}