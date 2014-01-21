using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace dnscrypt_winclient
{
	class ProvidersConfigurationSection : ConfigurationSection
	{
		// Declare the Urls collection property using the 
		// ConfigurationCollectionAttribute. 
		// This allows to build a nested section that contains 
		// a collection of elements.
		[ConfigurationProperty("providers", IsDefaultCollection = false)]
		[ConfigurationCollection(typeof(ProvidersCollection),
			AddItemName = "add",
			ClearItemsName = "clear",
			RemoveItemName = "remove")]
		public ProvidersCollection Providers
		{
			get
			{
				ProvidersCollection providersCollection = (ProvidersCollection)base["providers"];
				return providersCollection;
			}
		}
	}

	// Define the custom UrlsCollection that contains the  
	// custom UrlsConfigElement elements. 
	public class ProvidersCollection : ConfigurationElementCollection
	{
		public ProvidersCollection()
		{
		}

		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.AddRemoveClearMap;
			}
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new ProviderConfigElement();
		}

		protected override Object GetElementKey(ConfigurationElement element)
		{
			return ((ProviderConfigElement)element).Name;
		}

		public ProviderConfigElement this[int index]
		{
			get
			{
				return (ProviderConfigElement)BaseGet(index);
			}
			set
			{
				if (BaseGet(index) != null)
				{
					BaseRemoveAt(index);
				}
				BaseAdd(index, value);
			}
		}

		new public ProviderConfigElement this[string Name]
		{
			get
			{
				return (ProviderConfigElement)BaseGet(Name);
			}
		}

		public int IndexOf(ProviderConfigElement url)
		{
			return BaseIndexOf(url);
		}

		public void Add(ProviderConfigElement url)
		{
			BaseAdd(url);
		}
		protected override void BaseAdd(ConfigurationElement element)
		{
			BaseAdd(element, false);
		}

		public void Remove(ProviderConfigElement url)
		{
			if (BaseIndexOf(url) >= 0)
				BaseRemove(url.Name);
		}

		public void RemoveAt(int index)
		{
			BaseRemoveAt(index);
		}

		public void Remove(string name)
		{
			BaseRemove(name);
		}

		public void Clear()
		{
			BaseClear();
		}
	}

	// Define the custom UrlsConfigElement elements that are contained  
	// by the custom UrlsCollection. 
	// Notice that you can change the default values to create new default elements. 
	public class ProviderConfigElement : ConfigurationElement
	{
		public ProviderConfigElement()
		{
		}

		[ConfigurationProperty("name", DefaultValue = "", IsRequired = true, IsKey = true)]
		public string Name
		{
			get
			{
				return (string)this["name"];
			}
		}

		[ConfigurationProperty("server", DefaultValue = "", IsRequired = true)]
		public string Server
		{
			get
			{
				return (string)this["server"];
			}
		}

		[ConfigurationProperty("ipv6", DefaultValue = "", IsRequired = true)]
		public string Ipv6
		{
			get
			{
				return (string)this["ipv6"];
			}
		}

		[ConfigurationProperty("parental", DefaultValue = "", IsRequired = true)]
		public string Parental
		{
			get
			{
				return (string)this["parental"];
			}
		}

		[ConfigurationProperty("ports", DefaultValue = "", IsRequired = true)]
		public string Ports
		{
			get
			{
				return (string)this["ports"];
			}
		}

		[ConfigurationProperty("protos", DefaultValue = "", IsRequired = true)]
		public string Protos
		{
			get
			{
				return (string)this["protos"];
			}
		}

		[ConfigurationProperty("fqdn", DefaultValue = "", IsRequired = true)]
		public string FQDN
		{
			get
			{
				return (string)this["fqdn"];
			}
		}

		[ConfigurationProperty("key", DefaultValue = "", IsRequired = true)]
		public string Key
		{
			get
			{
				return (string)this["key"];
			}
		}
	}
}
