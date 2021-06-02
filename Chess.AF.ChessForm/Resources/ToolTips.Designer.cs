﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Chess.AF.ChessForm.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ToolTips {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ToolTips() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Chess.AF.ChessForm.Resources.ToolTips", typeof(ToolTips).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Positie van de stukken (gezien vanuit wit). Elke rij wordt beschreven, beginnend met de achtste rij en eindigend met de eerste rij. De rijen worden gescheiden door een slash (&quot;/&quot;). Binnen elke rij wordt de inhoud van elk veld beschreven van de a-lijn tot de h-lijn. Witte stukken worden aangegeven met hun Engelse aanduiding in hoofdletters (&quot;KQRBNP&quot;), zwarte stukken met diezelfde aanduiding in kleine letters (&quot;kqrbnp&quot;). Het aantal opeenvolgende lege velden op een rij wordt aangegeven met een cijfer dat van &quot; [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ttFen1 {
            get {
                return ResourceManager.GetString("ttFen1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Actieve kleur. &quot;w&quot; betekent wit aan zet, &quot;b&quot; betekent zwart aan zet..
        /// </summary>
        internal static string ttFen2 {
            get {
                return ResourceManager.GetString("ttFen2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rokademogelijkheid. &quot;-&quot; geeft aan dat geen rokade meer mogelijk is. Anders wordt één of meer van de volgende karakters gebruikt, in deze volgorde: &quot;K&quot; (wit kan op de koningsvleugel rokeren), &quot;Q&quot; (wit kan op de damevleugel rokeren), &quot;k&quot; (zwart kan op de koningsvleugel rokeren), &quot;q&quot; (zwart kan op de damevleugel rokeren)..
        /// </summary>
        internal static string ttFen3 {
            get {
                return ResourceManager.GetString("ttFen3", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to En-passantmogelijkheid. &quot;-&quot; geeft aan dat er niet en passant kan worden geslagen. Als een pion twee velden opschuift wordt het veld aangegeven dat door de pion werd overgeslagen. Bijvoorbeeld: na de zet e2-e4 wordt &quot;e3&quot; aangegeven. Merk op dat dit gebeurt ook als er geen pion van de andere partij is die en-passant kan slaan. Het cijfer is altijd 3 als wit heeft gezet en 6 als zwart heeft gezet..
        /// </summary>
        internal static string ttFen4 {
            get {
                return ResourceManager.GetString("ttFen4", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Halve-zettenteller. Geeft het aantal zetten sinds de laatste zet waarbij een pion is gezet of een stuk geslagen. Dit wordt gebruikt om te bepalen of een remiseclaim mogelijk is in verband met de vijftigzettenregel..
        /// </summary>
        internal static string ttFen5 {
            get {
                return ResourceManager.GetString("ttFen5", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Volledige-zettenteller. Het aantal volledige zetten. Dit begint met &quot;1&quot; en wordt opgehoogd na elke zet van zwart..
        /// </summary>
        internal static string ttFen6 {
            get {
                return ResourceManager.GetString("ttFen6", resourceCulture);
            }
        }
    }
}