namespace MoneyApp.Services
{
    public class CSVBuilder
    {
        private static string EscapeCsv(string? s, char sep)
        {
            s ??= "";
            bool mustQuote = s.Contains(sep) || s.Contains('"') || s.Contains('\n') || s.Contains('\r');
            if (s.Contains('"')) s = s.Replace("\"", "\"\"");
            return mustQuote ? $"\"{s}\"" : s;
        }

        // Public methods you will call from Blazor
        public string BuildGeneralCsv(IEnumerable<Models.GeneralEntryRow> rows)
            => BuildCsv(rows);

        public string BuildAuxCsv(IEnumerable<Models.AuxEntryRow> rows)
            => BuildCsv(rows);

        public string BuildBankCsv(IEnumerable<Models.BankStatementRow> rows)
            => BuildCsv(rows);

        // Keep the actual builders private (can stay static)
        private static string BuildCsv(IEnumerable<Models.GeneralEntryRow> rows)
        {
            char sep = ';';
            var sb = new System.Text.StringBuilder();
            sb.AppendLine(string.Join(sep, new[]
            {
                "Date","Compte","Description","Ref","Débit","Crédit"
            }.Select(x => EscapeCsv(x, sep))));

            foreach (var r in rows)
            {
                sb.AppendLine(string.Join(sep, new[]
                {
                    r.Date.ToString(),
                    r.Compte,
                    r.Description,
                    r.Ref,
                    r.Debit == 0 ? "" : r.Debit.ToString("0.00"),
                    r.Credit == 0 ? "" : r.Credit.ToString("0.00"),
                }.Select(x => EscapeCsv(x, sep))));
            }
            return sb.ToString();
        }

        private static string BuildCsv(IEnumerable<Models.AuxEntryRow> rows)
        {
            char sep = ';';
            var sb = new System.Text.StringBuilder();
            sb.AppendLine(string.Join(sep, new[]
            {
                "Compte","Date","Nom","Ref","Débit","Crédit"
            }.Select(x => EscapeCsv(x, sep))));

            foreach (var r in rows)
            {
                sb.AppendLine(string.Join(sep, new[]
                {
                    r.Compte,
                    r.Date.ToString(),
                    r.Nom,
                    r.Ref,
                    r.Debit == 0 ? "" : r.Debit.ToString("0.00"),
                    r.Credit == 0 ? "" : r.Credit.ToString("0.00"),
                }.Select(x => EscapeCsv(x, sep))));
            }
            return sb.ToString();
        }

        private static string BuildCsv(IEnumerable<Models.BankStatementRow> rows)
        {
            char sep = ';';
            var sb = new System.Text.StringBuilder();
            sb.AppendLine(string.Join(sep, new[]
            {
                "Date","Ref","Description","Nom","Montant","Statut"
            }.Select(x => EscapeCsv(x, sep))));

            foreach (var r in rows)
            {
                sb.AppendLine(string.Join(sep, new[]
                {
                    r.Date.ToString(),
                    r.Ref,
                    r.Description,
                    r.Nom,
                    r.Montant == 0 ? "" : r.Montant.ToString("0.00"),
                    r.Statut,
                }.Select(x => EscapeCsv(x, sep))));
            }
            return sb.ToString();
        }
    }
}
