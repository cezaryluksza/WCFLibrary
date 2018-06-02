using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LibraryAppService
{
    [ServiceContract]
    public interface ILibraryService
    {
        [OperationContract]
        List<Book> GetListOfBooks();
        [OperationContract]
        int VerifyLogin(string login, string password);
        [OperationContract]
        List<Libraries> BooksInLibraries(string ISBN);
        [OperationContract]
        bool Request(string login, string ISBN, int LibraryId);
        [OperationContract]
        List<Request> ShowRequests(string login);
        [OperationContract]
        List<PersonalData> GetPersonalData(string login);
    }

    [DataContract]
     public class Book
    {
        #region Fields
        private string isbn;
        private string bookTitle;
        private string publication;
        private int pages;
        private string series;
        #endregion

        #region Properties
        [DataMember]
        public string ISBN
        {
            get { return isbn; }
            set { isbn = value; }
        }
        [DataMember]
        public string BookTitle
        {
            get { return bookTitle; }
            set { bookTitle = value; }
        }
        [DataMember]
        public string Publication
        {
            get { return publication; }
            set { publication = value; }
        }
        [DataMember]
        public int Pages
        {
            get { return pages; }
            set { pages = value; }
        }
        [DataMember]
        public string Series
        {
            get { return series; }
            set { series = value; }
        }
        #endregion
    }
    [DataContract]
    public class Libraries
    {
        #region Fields
        private int libraryId;
        private string isbn;
        private int quantity;
        private string libraryName;
        private string street;
        private string numberBuilding;
        private string city;
        private string country;
        #endregion

        #region Properties

        [DataMember]
        public int LibraryId
        {
            get { return libraryId; }
            set { libraryId = value; }
        }
        [DataMember]
        public string ISBN
        {
            get { return isbn; }
            set { isbn = value; }
        }
        [DataMember]
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
        [DataMember]
        public string LibraryName
        {
            get { return libraryName; }
            set { libraryName = value; }
        }
        [DataMember]
        public string Street
        {
            get { return street; }
            set { street = value; }
        }
        [DataMember]
        public string NumberBuilding
        {
            get { return numberBuilding; }
            set { numberBuilding = value; }
        }
        [DataMember]
        public string City
        {
            get { return city; }
            set { city = value; }
        }
        [DataMember]
        public string Country
        {
            get { return country; }
            set { country = value; }
        }
        #endregion
    }
    public class Request
    {
        #region Fields
        private int requestId;
        private int memberId;
        private string isbn;
        private DateTime dateRequested;
        private DateTime dateLocated;
        private Book book;
        #endregion

        public Request()
        {
            book = new Book();
        }
        #region Properties
        [DataMember]
        public int RequestId
        {
            get { return requestId; }
            set { requestId = value; }
        }
        [DataMember]
        public int MemberId
        {
            get { return memberId; }
            set { memberId = value; }
        }
        [DataMember]
        public string ISBN
        {
            get { return isbn; }
            set { isbn = value; }
        }
        [DataMember]
        public DateTime DateRequested
        {
            get { return dateRequested; }
            set { dateRequested = value; }
        }
        [DataMember]
        public DateTime DateLocated
        {
            get { return dateLocated; }
            set { dateLocated = value; }
        }
        public Book PBook
        {
            get { return book; }
            set { book = value; }
        }
        #endregion

    }
    public class PersonalData
    {
        #region Fields
        private int memberId;
        private string firstName;
        private string lastName;
        private DateTime dateOfBirth;
        private string phoneNumber;
        private string email;
        private string street;
        private string numberBuilding;
        private string city;
        #endregion

        #region Properties
        [DataMember]
        public int MemberId
        {
            get { return memberId; }
            set { memberId = value; }
        }
        [DataMember]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        [DataMember]
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        [DataMember]
        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = value; }
        }
        [DataMember]
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }
        [DataMember]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        [DataMember]
        public string Street
        {
            get { return street; }
            set { street = value; }
        }
        [DataMember]
        public string NumberBuilding
        {
            get { return numberBuilding; }
            set { numberBuilding = value; }
        }
        [DataMember]
        public string City
        {
            get { return city; }
            set { city = value; }
        }
#endregion
    }
}
