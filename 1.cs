using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileShare.Business.Entities;
using FileShare.Business.Entities.Validators;
using System.Drawing;

namespace FileShare.Business.Logic
{
    public class FileSharing
    {
        public List<User> Users { get; set; }

        public List<Team> Teams { get; set; }

        public List<Board> Boards { get; set; }

        public FileSharing()
        {
            Users = new List<User>();
            Teams = new List<Team>();
            Boards = new List<Board>();
        }
        public List<User> GetAllUsers()
        {
            return this.Users;
        }

        public List<Team> GetAllTeams()
        {
            return this.Teams;
        }

        public List<Board> GetAllBoards()
        {
            return this.Boards;
        }
        public User GetUser(User user)
        {
            foreach (var item in this.Users)
            {
                if (item.Equals(user))
                {
                    return item;
                }
            }
            return null;
        }

        public Team GetTeam(Team team)
        {
            foreach (var item in this.Teams)
            {
                if (item.Equals(team))
                {
                    return item;
                }
            }
            return null;
        }

        public Board GetBoard(Board board)
        {
            foreach (var item in this.Boards)
            {
                if (item.Equals(board))
                {
                    return item;
                }
            }
            return null;
        }

        public Picture GetPicture(Picture pic)
        {
            Board board = GetBoard(pic.Container);
            if (board == null) return null;
            foreach (var item in board.Pictures)
            {
                if (item.Equals(pic))
                {
                    return item;
                }
            }
            return null;

        }

        public Text GetText(Text text)
        {
            Board board = GetBoard(text.Container);
            if (board == null) return null;
            foreach (var item in board.Texts)
            {
                if (item.Equals(text))
                {
                    return item;
                }
            }
            return null;

        }

        public Comment GetComment(Comment com)
        {
            if (com.Container.GetType() is Picture)
            {
                Picture pic = new Picture();
                pic.Id = com.Container.Id;
                Picture realPicture = this.GetPicture(pic);
                foreach (var item in realPicture.Comments)
                {
                    if (item.Equals(com))
                    {
                        return item;
                    }
                }
            }
            if (com.Container.GetType() is Text)
            {
                Text text = new Text();
                text.Id = com.Container.Id;
                Text realText = this.GetText(text);
                foreach (var item in realText.Comments)
                {
                    if (item.Equals(com))
                    {
                        return item;
                    }
                }
            }
            return null;
        }

        public bool CreateUser(User user)
        {
            UserValidation validUser = new UserValidation() { User = user };
            if (validUser.IsValidUser())
            {
                if (!Users.Contains(user))
                {
                    this.Users.Add(user);
                    return true;
                }
            }
            return false;

        }

        public bool CreateTeam(Team team)
        {
            TeamValidation validTeam = new TeamValidation() { Team = team };
            if (validTeam.IsValidTeam() && !Teams.Contains(team))
            {
                Teams.Add(team);
                return true;
            }
            return false;
        }

        public bool CreateBoard(Board board)
        {
            BoardValidation validBoard = new BoardValidation() { Board = board };
            if (validBoard.IsValidBoard() && !Boards.Contains(board))
            {
                this.Boards.Add(board);
                return true;
            }
            return false;
        }

        public bool CreateText(Text text)
        {
            Board theBoard = GetBoard(text.Container);
            if(theBoard != null)
            {
                int i = this.Boards.FindIndex(a => a == theBoard);
                if (!this.Boards[i].Texts.Contains(text))
                {
                    this.Boards[i].Texts.Add(text);
                    return true;
                }
            }
            return false;
        }

        public bool CreatePicture(Picture pic)
        {
            Board theBoard = GetBoard(pic.Container);
            if (theBoard != null)
            {
                int i = this.Boards.FindIndex(a => a == theBoard);
                if (!this.Boards[i].Pictures.Contains(pic))
                {
                    this.Boards[i].Pictures.Add(pic);
                    return true;
                }
            }
            return false;
        }

        public bool CreateComment(Comment comment)
        {
            if(comment.Container.GetType() is Picture)
            {
                Picture picture = (Picture)comment.Container;
                Board board = GetBoard(picture.Container);
                int i = this.Boards.FindIndex(a => a == board);
                int j = this.Boards[i].Pictures.FindIndex(b => b == picture);
                if (!this.Boards[i].Pictures[j].Comments.Contains(comment))
                {
                    this.Boards[i].Pictures[j].Comments.Add(comment);
                    return true;
                }

            }
            else
            {
                Text text = (Text)comment.Container;
                Board board = GetBoard(text.Container);
                int i = this.Boards.FindIndex(a => a == board);
                int j = this.Boards[i].Texts.FindIndex(b => b == text);
                if (!this.Boards[i].Texts[j].Comments.Contains(comment))
                {
                    this.Boards[i].Texts[j].Comments.Add(comment);
                    return true;
                }
            }
            return false;
        }

        public bool UpdateUser(User old, User update)
        {
            UserValidation validUser = new UserValidation() { User = update };
            if (old.Email != update.Email && Users.Contains(update)) return false;
            if (validUser.IsValidUser() && Users.Contains(old))
            {
                int i = this.Users.FindIndex(a => a.Email == old.Email);
                this.Users[i].Birthdate = update.Birthdate;
                this.Users[i].FirstName = update.FirstName;
                this.Users[i].Email = update.Email;
                this.Users[i].LastName = update.LastName;
                this.Users[i].Password = update.Password;
                this.Users[i].Type = update.Type;
                return true;
            }
            return false;
        }

        public bool UpdateTeam(Team old, Team update)
        {
            TeamValidation validTeam = new TeamValidation() { Team = update };
            if (old.Members != update.Members && Teams.Contains(update)) return false;
            if (validTeam.IsValidTeam() && Teams.Contains(old))
            {
                int i = this.Teams.FindIndex(a => a.Members == old.Members && a.Name == old.Name);
                this.Teams[i].Description = update.Description;
                this.Teams[i].MaxUsers = update.MaxUsers;
                this.Teams[i].Name = update.Name;
                this.Teams[i].Members = update.Members;
                return true;
            }
            return false;
        }

        public bool UpdateBoard(Board old, Board update)
        {
            BoardValidation validBoard = new BoardValidation() { Board = update };
            if (old != update && Boards.Contains(update)) return false;
            if (validBoard.IsValidBoard() && Boards.Contains(old))
            {
                int i = this.Boards.FindIndex(a => a.Name == old.Name && a.Creator == old.Creator && a.WorkTeam == old.WorkTeam);
                this.Boards[i].Dimension = update.Dimension;
                this.Boards[i].Description = update.Description;
                this.Boards[i].Creator = update.Creator;
                this.Boards[i].Name = update.Name;
                this.Boards[i].Pictures = update.Pictures;
                this.Boards[i].Texts = update.Texts;
                this.Boards[i].WorkTeam = update.WorkTeam;
                return true;
            }
            return false;
        }

        public bool UpdateText(Text old, Text update)
        {
            Board theBoard = GetBoard(old.Container);
            Text theText = GetText(old);
            if (theText != null)
            {
                int i = this.Boards.FindIndex(a => a == theBoard);
                int j = this.Boards[i].Texts.FindIndex(b => b == theText);
                this.Boards[i].Texts[j].Comments = update.Comments;
                this.Boards[i].Texts[j].Container = update.Container;
                this.Boards[i].Texts[j].Content = update.Content;
                this.Boards[i].Texts[j].Dimension = update.Dimension;
                this.Boards[i].Texts[j].Font = update.Font;
                this.Boards[i].Texts[j].Origin = update.Origin;
                return true;
            }
            return false;
        }

        public bool UpdatePicture(Picture old, Picture update)
        {
            Board theBoard = GetBoard(old.Container);
            Picture thePic = GetPicture(old);
            if (thePic != null)
            {
                int i = this.Boards.FindIndex(a => a == theBoard);
                int j = this.Boards[i].Pictures.FindIndex(b => b == thePic);
                this.Boards[i].Pictures[j].Comments = update.Comments;
                this.Boards[i].Pictures[j].Container = update.Container;
                this.Boards[i].Pictures[j].Dimension = update.Dimension;
                this.Boards[i].Pictures[j].ImageFile = update.ImageFile;
                this.Boards[i].Pictures[j].Origin = update.Origin;
                return true;
            }
            return false;
        }

        public bool ResolveComment(Comment comment)
        {
            comment.ResolveDate = DateTime.Today;
            comment.Status = "SOLVED";
           
            if (comment.Container.GetType() is Picture)
            {
                Picture picture = (Picture)comment.Container;
                Board board = GetBoard(picture.Container);
                int i = this.Boards.FindIndex(a => a == board);
                int j = this.Boards[i].Pictures.FindIndex(b => b == picture);
                int h = this.Boards[i].Pictures[j].Comments.FindIndex(c => c.Id == comment.Id);
                this.Boards[i].Pictures[j].Comments[h].ResolveDate = comment.ResolveDate;
                this.Boards[i].Pictures[j].Comments[h].ResolveUser = comment.ResolveUser;
                this.Boards[i].Pictures[j].Comments[h].Status = "SOLVED";
                return true;
            }
            else
            {
                Text text = (Text)comment.Container;
                Board board = GetBoard(text.Container);
                int i = this.Boards.FindIndex(a => a == board);
                int j = this.Boards[i].Texts.FindIndex(b => b == text);
                int h = this.Boards[i].Texts[j].Comments.FindIndex(c => c.Id == comment.Id);
                this.Boards[i].Texts[j].Comments[h].ResolveDate = comment.ResolveDate;
                this.Boards[i].Texts[j].Comments[h].ResolveUser = comment.ResolveUser;
                this.Boards[i].Texts[j].Comments[h].Status = "SOLVED";
                return true;
            }
           
            return false;
        }

        public bool DeleteUser(User user)
        {
            User theUser = GetUser(user);
            if (theUser!=null)
            {
                foreach(var item in this.Boards)
                {
                    if (item.Creator == user) return false;
                }
                foreach(var item in this.Teams)
                {
                    if (item.Members.Contains(user) && (item.Members.Count - 1 < 1)) return false;
                }
                Users.Remove(user);
                return true;
            }
            return false;
        }

        public bool DeleteTeam(Team team)
        {
            Team theTeam = GetTeam(team);
            if (theTeam != null)
            {
                foreach(var item in this.Boards)
                {
                    if (item.WorkTeam == team) return false;
                }
                Teams.Remove(team);
                return true;
            }
            return false;
        }

        public bool DeleteBoard(Board board) {
            Board theBoard = GetBoard(board);
            if (theBoard!=null)
            {
                Boards.Remove(board);
                return true;
            }
            return false;
        }

        public bool DeletePicture(Picture pic)
        {
            Board theBoard = GetBoard(pic.Container);
            if (theBoard!=null)
            {
                Picture thePicture = GetPicture(pic);
                if (thePicture!=null)
                {
                    theBoard.Pictures.Remove(thePicture);
                    return true;
                } 
            }
            return false;
        }

        public bool DeleteText(Text text)
        {
            Board theBoard = GetBoard(text.Container);
            if (theBoard!=null)
            {
                Text theText = GetText(text);
                if (theText !=null)
                {
                    theBoard.Texts.Remove(theText);
                    return true;
                }
            }
            return false;
        }

        public bool VerifyAccess(User user)
        {
            User actual = this.GetUser(user);
            if (actual == null) return false;
            if (actual.Password == user.Password) return true;
            return false;
        }
        

        public List<Board> FilterBoards(Team aTeam, DateTime? aCreationDate)
        {
            List<Board> result = new List<Board>();
            if (aTeam != null)
            {
                if (aCreationDate != null)
                {
                    foreach(var item in this.Boards)
                    {
                        if(item.CreationDate == aCreationDate && item.WorkTeam == aTeam)
                        {
                            result.Add(item);
                        }
                    }
                }
                else
                {
                    foreach (var item in this.Boards)
                    {
                        if (item.WorkTeam == aTeam)
                        {
                            result.Add(item);
                        }
                    }
                }
            }
            else if (aCreationDate != null)
                {
                    foreach (var item in this.Boards)
                    {
                        if (item.CreationDate == aCreationDate)
                        {
                            result.Add(item);
                        }
                    }
                }
                else
                {
                    return this.Boards;
                }
             return result;
          
           
            
        }

        public List<Comment> FilterResolvedComments(User aCreator, DateTime? aCreationDate, User aResolveUser, DateTime? aResolveDate)
        {
            List<Comment> result = new List<Comment>();
            foreach (var item in this.Boards)
            {
                foreach(var item2 in item.Pictures)
                {
                    foreach(var com in item2.Comments)
                    {
                        if(com.Status == "RESOLVED")
                        {
                            result.Add(com);
                        }
                    }
                }
                foreach (var item2 in item.Texts)
                {
                    foreach (var com in item2.Comments)
                    {
                        if (com.Status == "RESOLVED")
                        {
                            result.Add(com);
                        }
                    }
                }
            }

            if (aCreator != null) result = FilterByCreator(result, aCreator);
            if (aCreationDate != null) result = FilterByCreationDate(result, aCreationDate);
            if (aResolveUser != null) result = FilterByResolveUser(result, aResolveUser);
            if (aResolveDate != null) result = FilterByResolveDate(result, aResolveDate);
            return result;
        } 


        private List<Comment> FilterByCreator(List<Comment> aList, User aCreator)
        {
            List<Comment> result = new List<Comment>();
            foreach(var item in aList)
            {
                if (item.Creator == aCreator) result.Add(item);
            }
            return result;
        }
        private List<Comment> FilterByCreationDate(List<Comment> aList, DateTime? aCreationDate)
        {
            List<Comment> result = new List<Comment>();
            foreach (var item in aList)
            {
                if (item.CreationDate == aCreationDate) result.Add(item);
            }
            return result;
        }
        private List<Comment> FilterByResolveUser(List<Comment> aList, User aResolveUser)
        {
            List<Comment> result = new List<Comment>();
            foreach (var item in aList)
            {
                if (item.ResolveUser == aResolveUser) result.Add(item);
            }
            return result;
        }
        private List<Comment> FilterByResolveDate(List<Comment> aList, DateTime? aResolveDate)
        {
            List<Comment> result = new List<Comment>();
            foreach (var item in aList)
            {
                if (item.ResolveDate == aResolveDate) result.Add(item);
            }
            return result;
        }
        public List<Team> GetTeams(User aUser)
        {
            List<Team> result = new List<Team>();

            foreach (var item in this.Teams)
            {
                if (item.Members.Contains(aUser))
                {
                    result.Add(item);
                }
            }

            return result;
        }
        public List<Board> GetBoards(User aUser)
        {
            List<Board> result = new List<Board>();

            foreach(var item in Teams)
            {
                if (item.Members.Contains(aUser))
                {
                    List<Board> teamBoards = this.FilterBoards(item, null);
                    foreach (var item2 in teamBoards)
                    {
                        result.Add(item2);
                    }
                }
            }
            return result;
        }

        public bool ResetPassword(User aUser)
        {
            User reseted = new User();
            reseted.Birthdate = aUser.Birthdate;
            reseted.Email = aUser.Email;
            reseted.FirstName = aUser.FirstName;
            reseted.LastName = aUser.LastName;
            reseted.Password = "123";

            return UpdateUser(aUser, reseted);
        }
    }
}