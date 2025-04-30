using Smartstore.Core.Common.Configuration;
using Smartstore.Core.Configuration;
using Smartstore.Core.Security;
using Smartstore.Data.Migrations;

namespace Smartstore.Core.Data.Migrations
{
    public class SmartDbContextDataSeeder : IDataSeeder<SmartDbContext>
    {
        public DataSeederStage Stage => DataSeederStage.Early;
        public bool AbortOnFailure => false;

        public async Task SeedAsync(SmartDbContext context, CancellationToken cancelToken = default)
        {
            await context.MigrateLocaleResourcesAsync(MigrateLocaleResources);
            await MigrateSettingsAsync(context, cancelToken);
        }

        public async Task MigrateSettingsAsync(SmartDbContext context, CancellationToken cancelToken = default)
        {
            await SettingFactory.SaveSettingsAsync(context, new PerformanceSettings(), false);
            await SettingFactory.SaveSettingsAsync(context, new ResiliencySettings(), false);
        }

        public void MigrateLocaleResources(LocaleResourcesBuilder builder)
        {
            builder.AddOrUpdate("Admin.Configuration.Settings.Search.CommonFacet.Sorting",
                "Sorting",
                "Sortierung",
                "Specifies the sorting of the search filters.",
                "Legt die Sortierung der Suchfilter fest.");

            builder.AddOrUpdate("Enums.FacetSorting.ValueAsc", "Value/ID: lowest first", "Wert/ID: Niedrigste zuerst");

            builder.AddOrUpdate("Admin.Common.ExportToPdf.TooManyItems",
                "Too many objects! A maximum of {0} objects can be converted. Please reduce the number of selected data records ({1}) or increase the limit in the PDF settings.",
                "Zu viele Objekte! Es k�nnen maximal {0} Objekte konvertiert werden. Bitte reduzieren Sie die Anzahl der ausgew�hlten Datens�tze ({1}) oder erh�hen Sie das Limit in den PDF-Einstellungen.");

            builder.AddOrUpdate("Admin.Configuration.Settings.GeneralCommon.MaxItemsToPrint",
                "Maximum number of objects to print",
                "Maximale Anzahl zu druckender Objekte",
                "Specifies the maximum number of objects to be printed, above which an error message is issued. The default value is 500 and should not be set too high so that the process does not take too long.",
                "Legt die maximale Anzahl der zu druckenden Objekte fest, bei deren �berschreitung eine Fehlermeldung ausgegeben wird. Der Standardwert ist 500 und sollte nicht zu hoch gew�hlt werden, damit der Vorgang nicht zu lange dauert.");

            builder.AddOrUpdate("Admin.Configuration.Settings.Shipping.CalculateShippingAtCheckout",
                "Calculate shipping costs during checkout",
                "Versandkosten w�hrend des Checkouts berechnen",
                "Specifies whether shipping costs are displayed on the shopping cart page as long as the customer has not yet entered a shipping address. If activated, a note appears instead that the calculation will only take place at checkout.",
                "Legt fest, ob Versandkosten auf der Warenkorbseite angezeigt werden, solange der Kunde noch keine Lieferanschrift eingegeben hat. Wenn aktiviert, erscheint stattdessen ein Hinweis, dass die Berechnung erst beim Checkout erfolgt.");

            builder.AddOrUpdate("Common.CartRules", "Cart rules", "Warenkorbregeln");
            builder.AddOrUpdate("Common.CustomerRules", "Customer rules", "Kundenregeln");
            builder.AddOrUpdate("Common.ProductRules", "Product rules", "Produktregeln");

            builder.AddOrUpdate("Admin.Common.RestartHint",
                "Changes to the following settings only take effect after the application has been restarted.",
                "�nderungen an den folgenden Einstellungen werden erst nach einem Neustart der Anwendung wirksam.");

            builder.AddOrUpdate("Admin.Configuration.Settings.RewardPoints.RoundDownPointsForPurchasedAmount",
                "Round down the amount of points for a purchase",
                "Betrag bei Punkten f�r einen Einkauf abrunden",
                "Specifies whether to round down the amount when calculating the reward points awarded for a product purchase.",
                "Legt fest, ob der Betrag bei der Berechnung der Bonuspunkte, die f�r den Kauf eines Produkts gew�hrt werden, abgerundet werden soll.");

            builder.AddOrUpdate("Admin.Configuration.Settings.CustomerUser.HideMyAccountOrders",
                "Hide orders in the \"My account\" area",
                "Bestellungen im Bereich \"Mein Konto\" ausblenden");

            builder.AddOrUpdate("Admin.Rules.FilterDescriptor.VariantInCart", "Product with SKU in cart", "Produkt mit SKU im Warenkorb");

            builder.AddOrUpdate("Admin.RecurringPayments.History")
                .Value("de", "Historie");
            builder.AddOrUpdate("Admin.RecurringPayments.Fields.CyclesRemaining")
                .Value("de", "Verbleibende Zahlungen");
            builder.AddOrUpdate("Admin.RecurringPayments.Fields.CyclesRemaining.Hint")
                .Value("de", "Die Anzahl der verbleibenden Zahlungen");

            builder.AddOrUpdate("Admin.RecurringPayments.List.RemainingCycles",
                "Remaining payments",
                "Verbleibende Zahlungen",
                "Filter list by remaining payments.",
                "Liste nach verbleibenden Zahlungen filtern.");

            // Frontend renaming: "Wiederkehrende Zahlung" -> "Regelm��ige Lieferung".
            builder.AddOrUpdate("Account.CustomerOrders.RecurringOrders.Cancel", "Cancel repeat delivery", "Regelm��ige Lieferung abbrechen");
            builder.AddOrUpdate("Account.CustomerOrders.RecurringOrders", "Repeat deliveries", "Regelm��ige Lieferungen");
            builder.AddOrUpdate("Account.CustomerOrders.RecurringOrders.TotalCycles", "Total deliveries", "Lieferungen insgesamt");
            builder.AddOrUpdate("ShoppingCart.RecurringPeriod", "[Repeat deliveries every {0} {1}]", "[Regelm��ige Lieferung alle {0} {1}]");

            builder.AddOrUpdate("Account.CustomerOrders.RecurringOrders.CancelDelivery",
                "Would you like to cancel the repeat delivery?",
                "M�chten Sie die regelm��ige Lieferung abbrechen?");

            builder.AddOrUpdate("Account.CustomerOrders.RecurringOrders.SuccessfullyCanceled",
                "The repeat delivery was successfully canceled.",
                "Die regelm��ige Lieferung wurde erfolgreich abgebrochen.");

            builder.Delete(
                "Admin.RecurringPayments.History.OrderStatus",
                "Admin.RecurringPayments.History.PaymentStatus",
                "Admin.RecurringPayments.History.ShippingStatus",
                "Admin.Orders.Products.RecurringPeriod",
                "Account.CustomerOrders.RecurringOrders.ViewInitialOrder");

            builder.AddOrUpdate("Admin.Catalog.Products.RecycleBin.DeleteProductsResult",
                "{0} of {1} products have been permanently deleted.",
                "Es wurden {0} von {1} Produkten endg�ltig gel�scht.");

            builder.AddOrUpdate("Admin.Catalog.Products.RecycleBin.DeletedAndSkippedProductsResult",
                "{0} of {1} products have been permanently deleted. {2} Products were skipped as they are assigned to orders and cannot be permanently deleted.",
                "{0} von {1} Produkten wurden endg�ltig gel�scht. {2} Produkte wurden �bersprungen, da sie Auftr�gen zugeordnet sind und nicht permanent gel�scht werden k�nnen.");

            builder.AddOrUpdate("Order.CannotCompleteUnpaidOrder", 
                "An unpaid order cannot be completed.",
                "Ein unbezahlter Auftrag kann nicht abgeschlossen werden.");

            builder.Delete(
                "Admin.Orders.List.StartDate",
                "Admin.Orders.List.StartDate.Hint",
                "Admin.Orders.List.EndDate",
                "Admin.Orders.List.EndDate.Hint",
                "Admin.Customers.Reports.BestBy.StartDate",
                "Admin.Customers.Reports.BestBy.StartDate.Hint",
                "Admin.Customers.Reports.BestBy.EndDate",
                "Admin.Customers.Reports.BestBy.EndDate.Hint",
                "Admin.SalesReport.Bestsellers.StartDate",
                "Admin.SalesReport.Bestsellers.StartDate.Hint",
                "Admin.SalesReport.Bestsellers.EndDate",
                "Admin.SalesReport.Bestsellers.EndDate.Hint",
                "Admin.SalesReport.NeverSold.StartDate",
                "Admin.SalesReport.NeverSold.StartDate.Hint",
                "Admin.SalesReport.NeverSold.EndDate",
                "Admin.SalesReport.NeverSold.EndDate.Hint",
                "Admin.Orders.Shipments.List.StartDate",
                "Admin.Orders.Shipments.List.StartDate.Hint",
                "Admin.Orders.Shipments.List.EndDate",
                "Admin.Orders.Shipments.List.EndDate.Hint",
                "Admin.Common.Search.StartDate",
                "Admin.Common.Search.StartDate.Hint",
                "Admin.Common.Search.EndDate",
                "Admin.Common.Search.EndDate.Hint",
                "Admin.System.QueuedEmails.List.StartDate",
                "Admin.System.QueuedEmails.List.StartDate.Hint",
                "Admin.System.QueuedEmails.List.EndDate",
                "Admin.System.QueuedEmails.List.EndDate.Hint");

            builder.AddOrUpdate("Admin.Media.Editing.Align", "Align", "Ausrichten");
            
            builder.AddOrUpdate("Admin.Media.Editing.AlignTop", "Top", "Oben");
            builder.AddOrUpdate("Admin.Media.Editing.AlignMiddle", "Center", "Mitte");
            builder.AddOrUpdate("Admin.Media.Editing.AlignBottom", "Bottom", "Unten");


            #region Performance settings

            var prefix = "Admin.Configuration.Settings.Performance";

            builder.AddOrUpdate($"{prefix}", "Performance", "Leistung");
            builder.AddOrUpdate($"{prefix}.Resiliency", "Overload protection", "�berlastungsschutz");
            builder.AddOrUpdate($"{prefix}.Cache", "Cache", "Cache");

            builder.AddOrUpdate($"{prefix}.Hint",
                "For technically experienced users only. Pay attention to the CPU and memory usage when changing these settings.",
                "Nur f�r technisch erfahrene Benutzer. Achten Sie auf die CPU- und Speicherauslastung, wenn Sie diese Einstellungen �ndern.");

            builder.AddOrUpdate($"{prefix}.CacheSegmentSize",
                "Cache segment size", 
                "Cache Segment Gr��e",
                "The number of entries in a single cache segment when greedy loading is disabled. The larger the catalog, the smaller this value should be. We recommend segment size of 500 for catalogs with less than 100.000 items.",
                "Die Anzahl der Eintr�ge in einem einzelnen Cache-Segment, wenn Greedy Loading deaktiviert ist. Je gr��er der Katalog ist, desto kleiner sollte dieser Wert sein. Wir empfehlen eine Segmentgr��e von 500 f�r Kataloge mit weniger als 100.000 Eintr�gen.");

            builder.AddOrUpdate($"{prefix}.AlwaysPrefetchTranslations",
                "Always prefetch translations",
                "�bersetzungen immer vorladen (Prefetch)",
                "By default, only Instant Search prefetches translations. All other product listings work against the segmented cache. For very large multilingual catalogs (> 500,000), enabling this can improve query performance and reduce resource usage.",
                "Standardm��ig werden nur bei der Sofortsuche �bersetzungen vorgeladen. Alle anderen Produktauflistungen arbeiten mit dem segmentierten Cache. Bei sehr gro�en mehrsprachigen Katalogen (> 500.000) kann die Aktivierung dieser Option die Abfrageleistung verbessern und die Ressourcennutzung verringern.");

            builder.AddOrUpdate($"{prefix}.AlwaysPrefetchUrlSlugs",
                "Always prefetch URL slugs",
                "URL Slugs immer vorladen  (Prefetch)",
                "By default, only Instant Search prefetches URL slugs. All other product listings work against the segmented cache. For very large multilingual catalogs (> 500,000), enabling this can improve query performance and reduce resource usage.",
                "Standardm��ig werden nur bei der Sofortsuche URL slugs vorgeladen. Alle anderen Produktauflistungen arbeiten mit dem segmentierten Cache. Bei sehr gro�en mehrsprachigen Katalogen (> 500.000) kann die Aktivierung dieser Option die Abfrageleistung verbessern und die Ressourcennutzung verringern.");

            builder.AddOrUpdate($"{prefix}.MaxUnavailableAttributeCombinations",
                "Max. unavailable attribute combinations",
                "Max. nicht verf�gbare Attributkombinationen",
                "Maximum number of attribute combinations that will be loaded and parsed to make them unavailable for selection on the product detail page.",
                "Maximale Anzahl von Attributkombinationen, die geladen und analysiert werden, um nicht verf�gbare Kombinationen zu ermitteln.");

            builder.AddOrUpdate($"{prefix}.MediaDupeDetectorMaxCacheSize",
                "Media Duplicate Detector max. cache size",
                "Max. Cache-Gr��e f�r Medien-Duplikat-Detektor",
                "Maximum number of MediaFile entities to cache for duplicate file detection. If a media folder contains more files, no caching is done for scalability reasons and the MediaFile entities are loaded directly from the database.",
                "Maximale Anzahl der MediaFile-Entit�ten, die f�r die Duplikat-Erkennung zwischengespeichert werden. Enth�lt ein Medienordner mehr Dateien, erfolgt aus Gr�nden der Skalierbarkeit keine Zwischenspeicherung und die MediaFile-Entit�ten werden direkt aus der Datenbank geladen.");

            prefix = "Admin.Configuration.Settings.Resiliency";

            builder.AddOrUpdate($"{prefix}.Description",
                @"Overload protection is used to keep server resources under control, prevent latencies from getting out of hand and keep the system performant and available 
in the event of increased traffic (e.g. due to unidentifiable ""Bad Bots"", peaks, sales events, sudden high visitor numbers).
Limits only apply to guest accounts and bots, not to registered users.",
                @"�berlastungsschutz dient dazu, die Server-Ressourcen unter Kontrolle zu halten, Latenzen nicht ausufern zu lassen und im Falle von erh�htem Ansturm 
(z.B. durch nicht identifizierbare ""Bad-Bots"", Peaks, Sales Events, pl�tzlich hohe Nutzerzahlen) das System performant und verf�gbar zu halten.
Limits gelten nur f�r Gastkonten und Bots, nicht f�r registrierte User.");

            builder.AddOrUpdate($"{prefix}.LongTraffic", "Traffic limit", "Besucherlimit");
            builder.AddOrUpdate($"{prefix}.LongTrafficNotes",
                "Configuration of the long traffic window. Use these settings to monitor traffic restrictions over a longer period of time, such as a minute or longer.",
                "Konfiguration des langen Zeitfensters. Verwenden Sie diese Einstellungen, um Limits �ber einen l�ngeren Zeitraum zu �berwachen, z.B. eine Minute oder l�nger.");

            builder.AddOrUpdate($"{prefix}.PeakTraffic", "Peak", "Lastspitzen");
            builder.AddOrUpdate($"{prefix}.PeakTrafficNotes",
                "The peak traffic window defines the shorter period used for detecting sudden traffic spikes. These settings are useful for reacting to bursts of traffic that occur in a matter of seconds.",
                "Der k�rzere Zeitraum, der f�r die Erkennung pl�tzlicher Lastspitzen verwendet wird. Diese Einstellungen sind n�tzlich, um auf Lastspitzen zu reagieren, die innerhalb weniger Sekunden auftreten.");

            builder.AddOrUpdate($"{prefix}.TrafficTimeWindow",
                "Time window (hh:mm:ss)",
                "Zeitfenster (hh:mm:ss)",
                "The duration of the traffic window, which defines the period of time during which sustained traffic is measured.",
                "Die Dauer des Zeitfensters, das den Zeitraum definiert, in dem der anhaltende Traffic gemessen wird.");

            builder.AddOrUpdate($"{prefix}.TrafficLimitGuest",
                "Guest limit",
                "G�ste-Grenzwert",
                "The maximum number of requests allowed from guest users within the duration of the defined time window. Empty value means there is no limit applied for guest users.",
                "Die maximale Anzahl von Gastbenutzern innerhalb des festgelegten Zeitfensters. Ein leerer Wert bedeutet: keine Begrenzung.");

            builder.AddOrUpdate($"{prefix}.TrafficLimitBot",
                "Bot limit",
                "Bot-Grenzwert",
                "The maximum number of requests allowed from bots within the duration of the defined time window. Empty value means there is no limit applied for bots.",
                "Die maximale Anzahl von Bots innerhalb des festgelegten Zeitfensters. Ein leerer Wert bedeutet: keine Begrenzung.");

            builder.AddOrUpdate($"{prefix}.TrafficLimitGlobal",
                "Global limit",
                "Globaler Grenzwert",
                @"The global traffic limit for both guests and bots together. This limit applies to the combined traffic from guests and bots. 
It ensures that the overall system load remains within acceptable thresholds, regardless of the distribution of requests among specific user types.  
Unlike guest or bot limiters, this global limit acts as a safeguard for the entire system. If the cumulative requests from both types exceed this limit 
within the observation window, additional requests may be denied, even if type-specific limits have not been reached. An empty value means that there is no global limit.",
                @"Das globale Limit f�r G�ste und Bots zusammen. Dieses Limit gilt f�r den kombinierten Traffic von G�sten und Bots. 
Es stellt sicher, dass die Gesamtsystemlast innerhalb akzeptabler Schwellenwerte bleibt, unabh�ngig von der Verteilung der Anfragen auf bestimmte Benutzertypen. 
Im Gegensatz zu Gast- oder Bot-Limitern dient dieses globale Limit als Schutz f�r das gesamte System. 
Wenn die kumulierten Anfragen beider Typen dieses Limit innerhalb des Beobachtungsfensters �berschreiten, werden weitere Anfragen abgelehnt, 
auch wenn die typspezifischen Limits nicht erreicht wurden. Ein leerer Wert bedeutet: keine Begrenzung.");

            builder.AddOrUpdate($"{prefix}.EnableOverloadProtection",
                "Enable overload protection",
                "�berlastungsschutz aktivieren",
                "When enabled, the system applies the defined traffic limits and overload protection policies. If disabled, no traffic limits are enforced.",
                "Wendet die festgelegten Richtlinien an. Wenn diese Option deaktiviert ist, werden keine Begrenzungen erzwungen.");

            builder.AddOrUpdate($"{prefix}.ForbidNewGuestsIfSubRequest",
                "If sub request, forbid \"new\" guests",
                "Wenn Sub-Request, \"neue\" G�ste blockieren",
                @"Forbids ""new"" guest users if the request is a sub/secondary request, e.g., an AJAX request, POST, script, media file, etc. This setting can be used to restrict 
the creation of new guest sessions on successive (secondary) resource requests. A ""bad bot"" that does not accept cookies is difficult to identify 
as a bot and may create a new guest session with each (sub)-request, especially if it varies its client IP address and user agent string with each request. 
If enabled, new guests will be blocked under these circumstances.",
                @"Blockiert ""neue"" Gastbenutzer, wenn es sich bei der Anforderung um eine untergeordnete/sekund�re Anforderung handelt, z. B. AJAX, POST, Skript, Mediendatei usw. 
Diese Einstellung kann verwendet werden, um die Erstellung neuer Gastsitzungen bei aufeinander folgenden (sekund�ren) Ressourcenanfragen einzuschr�nken. 
Ein ""Bad Bot"", der keine Cookies akzeptiert, ist schwer als Bot zu erkennen und kann bei jeder (Unter-)Anfrage eine neue Gastsitzung erzeugen, 
insbesondere wenn er seine Client-IP-Adresse und den User-Agent-String bei jeder Anfrage �ndert. 
Wenn diese Option aktiviert ist, werden neue G�ste unter diesen Umst�nden blockiert.");

            #endregion

            builder.AddOrUpdate("Tax.LegalInfoShort3", "Prices {0}, {1}", "Preise {0}, {1}");

            builder.AddOrUpdate("Smartstore.AI.Prompts.PleaseContinue",
                "Continue exactly at the marked point without repeating the previous text.",
                "Fahre genau an der markierten Stelle fort, ohne den bisherigen Text zu wiederholen.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.ContinueHere", "[Continue here]", "[Fortsetzung hier]");

            builder.AddOrUpdate("Smartstore.AI.Prompts.Suggestions.Separation",
                "Separate the suggestions with the � character (paragraph mark).",
                "Trenne die Vorschl�ge durch das � Zeichen (Absatzmarke).");

            builder.AddOrUpdate("Smartstore.AI.Prompts.Suggestions.NoNumbering",
                "Do not use numbering.",
                "Verwende keine Nummerierungen.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.Suggestions.NoRepitions",
                "Each proposal must be unique - repetitions are not permitted.",
                "Jeder Vorschlag muss einzigartig sein � Wiederholungen sind nicht erlaubt.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.Suggestions.CharLimit",
                "Each suggestion should have a maximum of {0} characters.",
                "Jeder Vorschlag soll maximal {0} Zeichen haben.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.CharLimit",
                "The text can contain a maximum of {0} characters.",
                "Der Text darf maximal {0} Zeichen enthalten.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.CharWordLimit",
                "The text may contain no more than {0} characters and no more than {1} words.",
                "Der Text darf nicht mehr als {0} Zeichen und nicht mehr als {1} W�rter enthalten.");

            builder.AddOrUpdate("Admin.AI.Suggestions.DefaultPrompt",
                "Make {0} suggestions about the topic '{1}'.", 
                "Mache {0} Vorschl�ge zum Thema '{1}'.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.DontUseQuotes",
                "Do not enclose the text in quotation marks or other characters.",
                "Schlie�e den Text nicht in Anf�hrungszeichen oder andere Zeichen ein.");

            builder.AddOrUpdate("Admin.Orders.CompleteUnpaidOrder",
                "The order has a payment status of <strong>{0}</strong>. Do you still want to set it to complete?",
                "Der Auftrag hat den Zahlungsstatus <strong>{0}</strong>. M�chten Sie ihn trotzdem auf komplett setzen?");

            builder.AddOrUpdate("Products.Sorting.Featured")
                .Value("en", "Recommendation");

            builder.AddOrUpdate("Admin.Configuration.Settings.Search.UseFeaturedSorting",
                "Sort by recommendation",
                "Nach Empfehlung sortieren",
                "Specifies whether sorting by recommendations is offered instead of sorting by best results. If activated, the products are sorted in the order specified for them.",
                "Legt fest, ob die Sortierung nach Empfehlungen anstelle der Sortierung nach besten Ergebnissen angeboten wird. Wenn aktiviert, werden die Produkte in der f�r sie angegebenen Reihenfolge sortiert.");

            builder.AddOrUpdate("Admin.Catalog.Products.Fields.DisplayOrder",
                "Display order",
                "Reihenfolge",
                "Specifies the order in which associated products of a grouped product are displayed. In addition, this setting determines the order of hits in the search, if sort by recommendation is enabled in the search settings.",
                "Legt die Reihenfolge fest, in der verkn�pfte Produkte eines Gruppenproduktes angezeigt werden. Zus�tzlich legt diese Einstellung die Reihenfolge der Treffer bei der Suche fest, sofern in den Sucheinstellungen die Sortierung nach Empfehlung aktiviert ist.");

            builder.AddOrUpdate("Admin.Configuration.Settings.Shipping.FreeShippingCountryIds",
                "Countries with free shipping",
                "L�nder mit kostenlosem Versand",
                "Specifies the shipping countries for which free shipping is enabled. Free shipping is enabled for all countries if none are specified here (default).",
                "Legt die Lieferl�nder fest, f�r die der kostenlose Versand aktiviert ist. Wird hier kein Land angegeben, ist der kostenlose Versand f�r alle L�nder aktiv (Standardeinstellung).");

            builder.AddOrUpdate("Admin.Plugins.KnownGroup.AI", "AI", "KI");

            builder.AddOrUpdate("Admin.AI.TextCreation.Continue", "Continue", "Fortsetzen");

            builder.AddOrUpdate("Smartstore.AI.Prompts.AppendToLastSpan",
                "Be sure to append the new mark-up to the last span tag.",
                "F�ge das neue Mark-Up unbedingt an das letzte span-Tag an.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.PreserveOriginalText",
                "Return the complete text in your answer.",
                "Gib in deiner Antwort den vollst�ndigen Text zur�ck.");

            builder.AddOrUpdate("Admin.AI.EditHtml", "Edit HTML text", "HTML-Text bearbeiten");

            builder.AddOrUpdate("Smartstore.AI.Prompts.Role.Rules",
                "You must strictly follow these rules:",
                "Diese Regeln sind zwingend einzuhalten:");

            builder.AddOrUpdate("Smartstore.AI.Prompts.Role.HtmlEditor",
                "You are an intelligent AI editor for web content. You combine the skills of a professional copywriter and technical HTML editor. Your output must ALWAYS be valid HTML!",
                "Du bist ein intelligenter KI-Editor f�r Webinhalte. Du kombinierst die F�higkeiten eines professionellen Texters und technischen HTML-Editors. Deine Ausgabe ist IMMER valides HTML!");

            builder.AddOrUpdate("Smartstore.AI.Prompts.CreateHtml",
                "Return only pure HTML code",
                "Gib ausschlie�lich reinen HTML-Code zur�ck");

            builder.AddOrUpdate("Smartstore.AI.Prompts.DontUseMarkdown",
                "Do not use Markdown formatting, no backticks (```) and no indented code sections.",
                "Verwende keine Markdown-Formatierung, keine Backticks (```) und keine einger�ckten Codeabschnitte.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.CaretPos",
                "The placeholder [CARETPOS] marks the position where your new text should appear.",
                "Der Platzhalter [CARETPOS] markiert die Stelle, an der dein neuer Text erscheinen soll.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.ReturnNewTextOnly",
                "Return ONLY the newly created text - no original parts.",
                "Gib AUSSCHLIESSLICH den neu erstellten Text zur�ck - keine Originalbestandteile.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.WrapNewContentWithHighlightTag",
                "Any text that you generate or add must be enclosed in a real HTML <mark> tag. Example: <mark>additional sentence</mark> or <li><mark>additional list item</mark></li>. " +
                "The word 'mark' must never appear as visible text content.",
                "Umschlie�e jeden neu generierten Text mit einem echten <mark>-Tag. Beispiel: <mark>Zus�tzlicher Text</mark> oder <li><mark>Zus�tzlicher Listeneintrag</mark></li>. " +
                "Das Wort 'mark' darf niemals als sichtbarer Textinhalt erscheinen.");
                
            builder.AddOrUpdate("Smartstore.AI.Prompts.MissingCaretPosHandling",
                "If the placeholder [CARETPOS] is not included in the HTML, insert the new text at the end of the document.",
                "Wenn der Platzhalter [CARETPOS] im HTML nicht enthalten ist, f�ge den neuen Text am Ende des Dokuments ein.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.ValidHtmlOutput",
                "The generated output must be completely valid HTML that fits seamlessly into the existing HTML content.",
                "Die erzeugte Ausgabe muss vollst�ndig valides HTML sein, das sich nahtlos in den bestehenden HTML-Inhalt einf�gt.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.ContinueAtPlaceholder",
                "Determine the next higher block-level element that contains the placeholder [CARETPOS] (e.g. <p> or <div>). " +
                "Consider this element as a valid context area for your text addition. " +
                "If the user instruction requires it, the addition can also be made outside the caret position within this element.",
                "Ermittle das n�chsth�here Block-Level-Element, das den Platzhalter [CARETPOS] enth�lt (z.B. <p> oder <div>). " +
                "Betrachte dieses Element als g�ltigen Kontextbereich f�r deine Texterg�nzung. " +
                "Wenn die Benutzeranweisung es verlangt, kann die Erg�nzung auch au�erhalb der CaretPos innerhalb dieses Elements erfolgen.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.RemoveCaretPosPlaceholder",
                "Remove the placeholder [CARETPOS] completely. It must NEVER be included in the response - neither visibly nor as a control character. ",
                "Entferne den Platzhalter [CARETPOS] vollst�ndig. Er darf in der Antwort NIEMALS enthalten sein � weder sichtbar noch als Steuerzeichen.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.ReturnCompleteParentTag",
                "ALWAYS return the complete enclosing block-level parent element in which the new text was inserted or changed.",
                "Gib IMMER das vollst�ndige umschlie�ende Block-Level-Elternelement zur�ck, in dem der neue Text eingef�gt oder ver�ndert wurde.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.ReturnCompleteTable",
                "ALWAYS return the complete enclosing <table> tag in which the new text was inserted or changed.",
                "Gib IMMER das vollst�ndige table-Tag zur�ck, in dem der neue Text eingef�gt oder ver�ndert wurde.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.ReturnCompleteList",
                "ALWAYS return the complete tag of the list (<ul>, <ol> or <menu>) in which the new text was inserted or changed.",
                "Gib IMMER das vollst�ndige Tag der Liste (<ul>, <ol> oder <menu>) zur�ck, in dem der neue Text eingef�gt oder ver�ndert wurde.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.ReturnCompleteDefinitionList",
                "ALWAYS return the complete enclosing <dl> tag in which the new text was inserted or changed.",
                "Gib IMMER das vollst�ndige dl-Tag zur�ck, in dem der neue Text eingef�gt oder ver�ndert wurde.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.ReturnInstructionReinforcer",
                "ONLY return this one element - no other content before or after it.",
                "Gib NUR dieses eine Element zur�ck � keine anderen Inhalte davor oder danach."); 

            builder.AddOrUpdate("Smartstore.AI.Prompts.PreservePreviousHighlightTags",
                "Any text deviation from the transmitted original text must be enclosed with the mark tag. " +
                "When you create a new answer, take into account the text you added previously. " +
                "Enclose ANY text you have added within this chat history with the mark tag.",
                "Jegliche Text-Abweichung vom �bermittelten Originaltext muss mit dem mark-Tag umschlo�en werden. " +
                "Wenn du eine neue Antwort erstellst, ber�cksichtige den Text, den du zuvor hinzugef�gt hast. " +
                "Umschlie�e JEDEN Text, der von dir innerhalb dieses Chat-Verlaufes hinzugef�gt wurde, mit dem mark-Tag.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.ContinueTable",
                "If the user requests a table extension, use [CARETPOS] exclusively to localize the table. " +
                "Expand the table logically without continuing directly at the caret position - unless the user explicitly requests that the current cell be edited.",
                "Wenn der User eine Tabellenerweiterung verlangt, verwende [CARETPOS] ausschlie�lich zur Lokalisierung der Tabelle. " +
                "Erg�nze die Tabelle logisch, ohne direkt an der Caret-Position weiterzuschreiben � es sei denn, der User fordert ausdr�cklich eine Bearbeitung der aktuellen Zelle.");

            builder.AddOrUpdate("Admin.Catalog.Products.List.SearchWithOrders",
                "With order assignments",
                "Mit Auftragszuordnungen",
                "Filters for products with/without order assignments.",
                "Filtert nach Produkten mit/ohne Auftragszuordnungen.");

            builder.AddOrUpdate("Admin.Customers.CookieConsent", "Cookie consent", "Cookie-Zustimmung");

            builder.AddOrUpdate("Admin.Customers.CookieConsent.ConsentOn",
                "Cookie consent on",
                "Cookie-Zustimmung am",
                "The date of the customer's consent to the use of cookies.",
                "Das Datum, an dem der Kunde der Verwendung von Cookies zugestimmt hat.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.NoFriendlyIntroductions",
                "Don't start your answer with meta-comments or introductions like: 'Gladly, here's your HTML'.",
                "Erstelle keine Meta-Kommentare oder Einleitungen wie: 'Gerne, hier ist dein HTML.'");
            
            // Changed for more precision.
            builder.AddOrUpdate("Smartstore.AI.Prompts.StartWithDivTag",
                "Do not create a complete HTML document - the output must start with a <div> tag.",
                "Erstelle kein vollst�ndiges HTML-Dokument - die Ausgabe muss mit einem <div>-Tag beginnen.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.DontCreateProductTitle",
                "Do not create a heading that contains the product name.",
                "Erstelle keine �berschrift, die den Produktnamen enth�lt.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.DontCreateTitle",
                "Do not create the title: '{0}'.",
                "Erstelle nicht den Titel: '{0}'.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.WriteCompleteParagraphs",
                "Create a complete and coherent text for each section.",
                "Erstelle f�r jeden Abschnitt einen inhaltlich vollst�ndigen und zusammenh�ngenden Text.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.UseImagePlaceholders",
                "If an image is to be inserted, use a <div class=�mb-3�> with an <i> tag containing the classes 'far fa-xl fa-file-image ai-preview-file' as a placeholder. " +
                "The title attribute must correspond to the associated section heading.",
                "Wenn ein Bild einzuf�gen ist, verwende als Platzhalter ein <div class=\"mb-3\"> mit einem <i>-Tag, " +
                "das die Klassen 'far fa-xl fa-file-image ai-preview-file' enth�lt. Das title-Attribut muss der zugeh�rigen Abschnitts�berschrift entsprechen.");

            // INFO: Minor change from "Be a ..." to "You are a ...". Seams irrelevant for a human reader, but it's important for the role understanding of the AI.
            builder.AddOrUpdate("Smartstore.AI.Prompts.Role.Translator",
                "Be a professional translator.",
                "Du bist ein professioneller �bersetzer.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.Role.Copywriter",
                "Be a professional copywriter.",
                "Du bist ein professioneller Texter.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.Role.Marketer",
                "Be a marketing expert.",
                "Du bist ein Marketing-Experte.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.Role.SEOExpert",
                "Be a SEO expert.",
                "Du bist ein SEO-Experte.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.Role.Blogger",
                "Be a professional blogger.",
                "Du bist ein professioneller Blogger.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.Role.Journalist",
                "Be a professional journalist.",
                "Du bist ein professioneller Journalist.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.Role.SalesPerson",
                "Be an assistant who creates product descriptions that convince a potential customer to buy.",
                "Du bist ein Assistent bei der Erstellung von Produktbeschreibungen, die einen potentiellen Kunden zum Kauf �berzeugen.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.Role.ProductExpert",
                "Be an expert for the product: '{0}'.",
                "Du bist ein Experte f�r das Produkt: '{0}'.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.Role.ImageAnalyzer",
                "Be an image analyzer assistant.",
                "Du bist ein Assistent f�r Bildanalyse.");

            // We change this resource to be a pure user message her. The main instruction of this order was shifted to the role description.
            // INFO: Not only is this the preferred method for engineering prompts, but it also reduces the custom prompt message to a minimum.
            builder.AddOrUpdate("Smartstore.AI.Prompts.IncludeImages",
                "Insert an image after each paragraph.",
                "F�ge nach jedem Absatz ein Bild ein.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.Language",
                "Write in {0}.",
                "Schreibe auf {0}.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.MainHeadingTag",
                "Use a {0} tag for the main heading.",
                "Nutze f�r die Haupt�berschrift ein {0}-Tag.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.ParagraphHeadingTag",
                "Use {0} tags for the paragraph headings.",
                "Nutze f�r die �berschriften der Abschnitte {0}-Tags.");

            builder.AddOrUpdate("Admin.Configuration.Settings.GeneralCommon.XmlSitemapIncludesAlternateLinks",
                "Add alternate links for localized pages",
                "Alternate Links f�r lokalisierte Seiten hinzuf�gen",
                "Specifies whether to add alternate links (xhtml:link) for localized page versions to the XML Sitemap.",
                "Legt fest, ob Alternate Links (xhtml:link) f�r lokalisierte Seitenversionen in der XML-Sitemap hinzugef�gt werden sollen.");

            builder.AddOrUpdate("Admin.Configuration.Settings.GeneralCommon.AddAlternateHtmlLinks",
                "Add alternate links for localized pages",
                "Alternate Links f�r lokalisierte Seiten hinzuf�gen",
                "Specifies whether to add alternate links (link rel='alternate') for localized page versions in the HTML header.",
                "Legt fest, ob Alternate Links (link rel='alternate') f�r lokalisierte Seitenversionen in den HTML-Header eingef�gt werden sollen.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.ImageAnalyzer.ObjectDefinition",
                "Return exactly one single JSON object with these keys and meanings:",
                "Gib genau ein einzelnes JSON-Objekt mit diesen Keys und Bedeutungen zur�ck:");

            builder.AddOrUpdate("Smartstore.AI.Prompts.ImageAnalyzer.ObjectDefinition.Title",
                "'title': short, precise description of the image content for the HTML title attribute (max. 60 characters)",
                "'title': kurze, pr�zise Beschreibung des Bildinhalts f�r das HTML-title-Attribut (max. 60 Zeichen)");

            builder.AddOrUpdate("Smartstore.AI.Prompts.ImageAnalyzer.ObjectDefinition.Alt",
                "'alt': clearly legible description of the image content for the HTML alt attribute",
                "'alt': klar lesbare Beschreibung des Bildinhalts f�r das HTML-alt-Attribut");

            builder.AddOrUpdate("Smartstore.AI.Prompts.ImageAnalyzer.ObjectDefinition.Tags",
                "'tags': exactly 5 thematically matching terms, as a comma-separated list",
                "'tags': exakt 5 thematisch passende Begriffe, als kommagetrennte Liste");

            builder.AddOrUpdate("Smartstore.AI.Prompts.ImageAnalyzer.NoContent",
                "Set the value 'no-content' in every field for which no meaningful content can be determined.",
                "Setze den Wert 'no-content' in jedem Feld, f�r das kein sinnvoller Inhalt ermittelt werden kann.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.CreateJson",
                "Only return a single JSON object - without formatting, meta comments or additional text.",
                "Gib ausschlie�lich ein einziges JSON-Objekt zur�ck � ohne Formatierungen, Meta-Kommentare oder zus�tzlichen Text.");

            builder.Delete("Smartstore.AI.Prompts.JustHtml");

            builder.AddOrUpdate("Smartstore.AI.Prompts.Translator.TranslateTextContentOnly",
                "Translate text content between HTML tags as well as plain text without HTML structure.",
                "�bersetze Textinhalte zwischen HTML-Tags sowie reinen Flie�text ohne HTML-Struktur.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.Translator.PreserveHtmlStructure",
                "Do not alter any HTML tags. Do not add, remove, or restructure tags in any way.",
                "Ver�ndere keine HTML-Tags. F�ge keine Tags hinzu, entferne keine und �ndere keine Verschachtelung.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.Translator.IgnoreTechnicalAttributes",
                "Do not translate attribute values that serve technical purposes, such as href, src, id, class, style, or data-*.",
                "�bersetze keine Attributwerte, die technische Funktionen erf�llen, z.B.: 'href, src, id, class, style, data-*'.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.Translator.KeepHtmlEntitiesIntact",
                "Do not modify HTML entities (e.g., &nbsp;, &copy;, &ndash;) in meaning or form.",
                "Ver�ndere HTML-Entities (z.B. &nbsp;, &copy;, &ndash;) nicht � weder semantisch noch formal.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.Translator.TranslateWithContext",
                "When translating text within inline tags (e.g., <strong>, <em>, <span>, <a>, �), always preserve the full sentence context.",
                "Ber�cksichtige beim �bersetzen von Textteilen in Inline-Tags (z.B. <strong>, <em>, <span>, <a>, �) immer den vollst�ndigen Satzkontext.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.Translator.TranslateDescriptiveAttributes",
                "Translate attribute values that convey information to the reader, such as alt and title.",
                "�bersetze Attributwerte, die dem Leser Informationen vermitteln, z.B. alt und title.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.Translator.PreserveToneAndStyle",
                "Preserve the tone and style of the original text. Do not simplify, paraphrase, or smooth the language.",
                "Behalte den Tonfall und Stil des Ausgangstexts bei. Verwende keine stilistischen Gl�ttungen, Umschreibungen oder Vereinfachungen.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.Translator.SkipAlreadyTranslated",
                "If the text is already in the target language, return it unchanged.",
                "Wenn der Text bereits in der Zielsprache vorliegt, gib ihn unver�ndert zur�ck.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.Translator.NoMetaComments",
                "Do not add explanations or meta comments (e.g., 'The text is already in English.').",
                "F�ge keine Erkl�rungen oder Meta-Kommentare hinzu (z.B. 'Der Text ist schon Englisch.').");

            builder.AddOrUpdate("Smartstore.AI.Prompts.Product.NoAssumptions",
                "Only describe what is clearly known about the product. Do not make any assumptions about the product.",
                "Beschreibe nur, was von dem Produkt eindeutig bekannt ist. Stelle keine Vermutungen �ber das Produkt an.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.OneParagraph",
                "The text contains only one section, which is enclosed by a p-tag.",
                "Der Text beinhaltet nur einen Abschnitt, der von einem p-Tag umschlossen ist.");

            builder.AddOrUpdate("Admin.Customers.DeleteCustomer", "Delete customer", "Kunde l�schen");

            builder.Delete("Account.PasswordRecovery.EmailNotFound");

            builder.AddOrUpdate("Account.PasswordRecovery.EmailHasBeenSent",
                "We have sent you an email with further instructions if an account exists with your email address.",
                "Wir haben Ihnen eine E-Mail mit weiteren Anweisungen geschickt, falls ein Konto mit Ihrer E-Mail-Adresse existiert.");

            builder.AddOrUpdate("Admin.DataExchange.Import.UpdateAllKeyFieldMatches",
                "Update all that match a key field value",
                "Alle aktualisieren, die dem Wert eines Schl�sselfelds entsprechen",
                "Specifies that all records matching the value of a key field are updated. By default, only the first matching record is updated." +
                " Enable this option if, for example, you have assigned an MPN multiple times and want to update all products with that MPN in a consistent manner." +
                " Enabling this option may reduce the performance of the import.",
                "Legt fest, dass alle mit dem Wert eines Schl�sselfeldes �bereinstimmenden Datens�tze aktualisiert werden. Standardm��ig wird nur der erste �bereinstimmende" +
                " Datensatz aktualisiert. Aktivieren Sie diese Option, wenn Sie z.B. eine MPN mehrfach vergeben haben und alle Produkte mit dieser MPN einheitlich aktualisieren m�chten." +
                " Die Aktivierung dieser Option kann die Performance des Imports beeintr�chtigen.");

            // Fix:
            builder.AddOrUpdate("Admin.DataExchange.Import.KeyFieldNames.Note")
                .Value("de", "Bitte verwenden Sie das Feld ID nur dann als Schl�sselfeld, wenn die Daten aus derselben Datenbank stammen, in die sie importiert werden sollen. Andernfalls k�nnen falsche Datens�tze aktualisiert werden.");

            builder.AddOrUpdate("Admin.Configuration.Settings.Shipping.UseShippingOriginIfShippingAddressMissing",
                "Shipping origin determines shipping costs if shipping address is missing",
                "Artikelstandort bestimmt Versandkosten bei fehlender Versandanschrift",
                "Specifies that if the customer has never checked out and the shipping address is unknown, the shipping cost from the location where the order was shipped (according to \"Shipping Origin\") will be used.",
                "Legt fest, dass die Versandkosten des Ortes verwendet werden, von dem aus der Versand erfolgt (gem�� \"Versand erfolgt ab\"), sofern der Kunde den Checkout noch nie durchlaufen hat und seine Versandanschrift unbekannt ist.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.ReserveSpaceForShopName",
                "When creating text for title tags, do not use the name of the website, as this will be added later - Reserve {0} characters for this.",
                "Verwende bei der Erstellung von Texten f�r title-Tags nicht den Namen der Website, da dieser sp�ter hinzugef�gt wird - Reserviere daf�r {0} Zeichen.");
        }
    }
}