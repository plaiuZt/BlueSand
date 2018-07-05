using System;

namespace Git.Framework.MsSql.Entity
{
    public class ReferencedEntityAttribute : Attribute
    {
        private string m_ConditionProperty;
        private string m_Prefix;
        private System.Type m_Type;

        public ReferencedEntityAttribute(System.Type type)
        {
            this.m_Type = type;
        }

        public string ConditionalProperty
        {
            get
            {
                return this.m_ConditionProperty;
            }
            set
            {
                this.m_ConditionProperty = value;
            }
        }

        public string Prefix
        {
            get
            {
                return this.m_Prefix;
            }
            set
            {
                this.m_Prefix = value;
            }
        }

        public System.Type Type
        {
            get
            {
                return this.m_Type;
            }
        }
    }
}

