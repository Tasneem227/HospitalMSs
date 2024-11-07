namespace HospitalMS.ViewModel
{
    
        public class BookingNurseViewModel
        {
            public TimeOnly Time { get; set; }
            public DateOnly Date { get; set; }
            public string? PatientFirstName { get; set; }
            public string? PatientLastName { get; set; }
            public string DoctorFirstName { get; set; }
            public string DoctorLastName { get; set; }
            public string DoctorImage {  get; set; }  
        
    }
}
