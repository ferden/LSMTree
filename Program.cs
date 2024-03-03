using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        var lsmTree = new LsmTree(basePath, 30);

        InitializeIller(lsmTree);

        Console.WriteLine("LSM Tree Uygulamasına Hoşgeldiniz.");
        Console.WriteLine("Kullanılabilir komutlar: add, update, delete, get, exit");

        string input;
        while (true)
        {
            Console.Write("> ");
            input = Console.ReadLine();
            if (string.IsNullOrEmpty(input)) continue;

            var parts = input.Split(' ');
            if (parts.Length < 2)
            {
                Console.WriteLine("Eksik argüman.");
                continue;
            }

            var command = parts[0].ToLower();
            switch (command)
            {
                case "add":
                case "update":
                    if (parts.Length < 3) { Console.WriteLine("Eksik argüman."); break; }
                    lsmTree.AddOrUpdate(parts[1], parts[2]);
                    Console.WriteLine($"'{parts[1]}' için değer '{parts[2]}' olarak ayarlandı.");
                    break;
                case "delete":
                    lsmTree.Delete(parts[1]);
                    Console.WriteLine($"'{parts[1]}' silindi.");
                    break;
                case "get":
                    var value = lsmTree.Get(parts[1]);
                    Console.WriteLine(value != null ? $"{parts[1]}: {value}" : "Bulunamadı.");
                    break;
                case "exit":
                    Console.WriteLine("Çıkılıyor...");
                    return;
                default:
                    Console.WriteLine("Geçersiz komut.");
                    break;
            }
        }
    }

    static void InitializeIller(LsmTree lsmTree)
    {
        var iller = new Dictionary<string, string>
        {
            {"01", "Adana"}, {"02", "Adıyaman"}, {"03", "Afyonkarahisar"}, {"04", "Ağrı"}, {"05", "Amasya"},
            {"06", "Ankara"}, {"07", "Antalya"}, {"08", "Artvin"}, {"09", "Aydın"}, {"10", "Balıkesir"},
            {"11", "Bilecik"}, {"12", "Bingöl"}, {"13", "Bitlis"}, {"14", "Bolu"}, {"15", "Burdur"},
            {"16", "Bursa"}, {"17", "Çanakkale"}, {"18", "Çankırı"}, {"19", "Çorum"}, {"20", "Denizli"},
            {"21", "Diyarbakır"}, {"22", "Edirne"}, {"23", "Elazığ"}, {"24", "Erzincan"}, {"25", "Erzurum"},
            {"26", "Eskişehir"}, {"27", "Gaziantep"}, {"28", "Giresun"}, {"29", "Gümüşhane"}, {"30", "Hakkâri"},
            {"31", "Hatay"}, {"32", "Isparta"}, {"33", "Mersin"}, {"34", "İstanbul"}, {"35", "İzmir"},
            {"36", "Kars"}, {"37", "Kastamonu"}, {"38", "Kayseri"}, {"39", "Kırklareli"}, {"40", "Kırşehir"},
            {"41", "Kocaeli"}, {"42", "Konya"}, {"43", "Kütahya"}, {"44", "Malatya"}, {"45", "Manisa"},
            {"46", "Kahramanmaraş"}, {"47", "Mardin"}, {"48", "Muğla"}, {"49", "Muş"}, {"50", "Nevşehir"},
            {"51", "Niğde"}, {"52", "Ordu"}, {"53", "Rize"}, {"54", "Sakarya"}, {"55", "Samsun"},
            {"56", "Siirt"}, {"57", "Sinop"}, {"58", "Sivas"}, {"59", "Tekirdağ"}, {"60", "Tokat"},
            {"61", "Trabzon"}, {"62", "Tunceli"}, {"63", "Şanlıurfa"}, {"64", "Uşak"}, {"65", "Van"},
            {"66", "Yozgat"}, {"67", "Zonguldak"}, {"68", "Aksaray"}, {"69", "Bayburt"}, {"70", "Karaman"},
            {"71", "Kırıkkale"}, {"72", "Batman"}, {"73", "Şırnak"}, {"74", "Bartın"}, {"75", "Ardahan"},
            {"76", "Iğdır"}, {"77", "Yalova"}, {"78", "Karabük"}, {"79", "Kilis"}, {"80", "Osmaniye"},
            {"81", "Düzce"}
        };

        foreach (var il in iller)
        {
            lsmTree.AddOrUpdate(il.Key, il.Value);
        }
    }

    class LsmTree
    {
        private Dictionary<string, string> memTable = new Dictionary<string, string>();
        private readonly string basePath;
        private readonly int memTableLimit;
        private List<string> sstables = new List<string>();

        public LsmTree(string basePath, int memTableLimit)
        {
            this.basePath = basePath;
            this.memTableLimit = memTableLimit;
            LoadExistingSSTables();
        }

        public void AddOrUpdate(string key, string value)
        {
            memTable[key] = value;
            if (memTable.Count >= memTableLimit)
            {
                FlushMemTableToDisk();
            }
        }

        public void Delete(string key)
        {
            memTable[key] = null; // Silme işlemini işaretlemek için değeri null olarak ayarla.
        }

        public string Get(string key)
        {
            // Öncelikle MemTable'da ara
            if (memTable.TryGetValue(key, out var value))
            {
                return value;
            }
            // MemTable'da bulunamazsa, SSTable'larda ara
            foreach (var sstable in sstables)
            {
                var lines = File.ReadAllLines(sstable);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts[0] == key && parts.Length > 1)
                    {
                        return parts[1];
                    }
                }
            }
            return null;
        }

        private void FlushMemTableToDisk()
        {
            var sstablePath = Path.Combine(basePath, $"sstable_{sstables.Count}.csv");
            using (var writer = new StreamWriter(sstablePath, false, Encoding.UTF8))
            {
                foreach (var kvp in memTable)
                {
                    if (kvp.Value != null) // Silinmiş değerleri dikkate alma
                    {
                        writer.WriteLine($"{kvp.Key},{kvp.Value}");
                    }
                }
            }
            memTable.Clear();
            sstables.Add(sstablePath);
        }

        private void LoadExistingSSTables()
        {
            var existingFiles = Directory.GetFiles(basePath, "sstable_*.csv");
            sstables.AddRange(existingFiles.OrderBy(filename => filename));
        }
    }
}
