using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace MyFirstWindowsApp.DataModel
{
    public class Instructor
    {
        public string Name { get; set; }
        public string ImageUri { get; set; }
    }

    public class Occurrence
    {
        public ObservableCollection<Instructor> Instructors;
        public ParticipantList Participants;
        public string Instance;
        public string Title;
        public bool CheckIn;

        public Occurrence()
        {
            Instructors = new ObservableCollection<Instructor>();
            Participants = new ParticipantList();
        }

        public Occurrence(string instance, string title, bool checkIn = false) : this()
        {
            Instance = instance;
            Title = title;
            CheckIn = checkIn;
        }
    }

    public class ParticipantsDataSource
    {
        private static ParticipantsDataSource _participantDataSource = new ParticipantsDataSource();
        private ObservableCollection<Occurrence> _occurrences = new ObservableCollection<Occurrence>();

        public ObservableCollection<Occurrence> Occurrences
        {
            get { return _occurrences; }
        }

        public async Task GetParticipantSampleData()
        {
            if (Occurrences.Any())
            {
                return;
            }

            var dataUri = new Uri("ms-appx:///DataModel/ParticipantSample.json");

            var file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            var jsonText = await FileIO.ReadTextAsync(file);
            var jsonObject = JsonObject.Parse(jsonText);
            var jsonArray = jsonObject["Occurrences"].GetArray();

            foreach (var occurrenceValue in jsonArray)
            {
                var occurrenceObject = occurrenceValue.GetObject();
                var occurrence = new Occurrence(occurrenceObject["Instance"].GetString(), 
                    occurrenceObject["Title"].GetString());

                foreach(var instructorValue in occurrenceObject["Instructors"].GetArray())
                {
                    var instructorObject = instructorValue.GetObject();
                    occurrence.Instructors.Add(new Instructor
                    {
                        Name = instructorObject["Name"].GetString(),
                        ImageUri = instructorObject["ImageUri"].GetString()
                    });
                }
                foreach(var participantValue in occurrenceObject["Participants"].GetArray())
                {
                    var participantObject = participantValue.GetObject();
                    occurrence.Participants.Add(new Participant
                    {
                        Name = participantObject["Name"].GetString(),
                        Image = participantObject["Image"].GetString(),
                        Email = participantObject["Email"].GetString()
                    });
                }
                
                Occurrences.Add(occurrence);
            }
        }

        public static async Task<IEnumerable<Occurrence>> GetClassesAsync()
        {
            await _participantDataSource.GetParticipantSampleData();

            return _participantDataSource.Occurrences;
        }

        public static async Task<Occurrence> GetClassInstanceAsync(string instanceId)
        {
            await _participantDataSource.GetParticipantSampleData();
            var matches = _participantDataSource.Occurrences.Where(
                o => o.Instance.ToLower().Equals(instanceId.ToLower())).ToList();

            return matches.Any() ? matches.First() : null;
        }

        public static async Task<IEnumerable<Participant>> GetClassParticipantsAsync(string instance)
        {
            var occurrence = await GetClassInstanceAsync(instance);

            return occurrence != null ? occurrence.Participants : null;
        }
    }
}
