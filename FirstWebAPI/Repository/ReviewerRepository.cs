using FirstWebAPI.Data;
using FirstWebAPI.Interfaces;
using FirstWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstWebAPI.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _context;
        public ReviewerRepository(DataContext context) 
        {
            _context = context;
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            _context.Reviewers.Add(reviewer);
            return Save();
        }

        //Also deletes all entries in Reviews that reference reviewerId
        //(cascading behaviour)
        public bool DeleteReviewer(int reviewerId)
        {
            var reviewer = _context.Reviewers.FirstOrDefault(re => re.Id == reviewerId);
            _context.Reviewers.Remove(reviewer);
            return Save();
        }

        public Reviewer GetReviewer(int reviewerId)
        {
            return _context.Reviewers.Where(r => r.Id == reviewerId).Include(r => r.Reviews).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _context.Reviewers.ToList();
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
            return _context.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToList();
        }

        public bool ReviewerExists(int reviewerId)
        {
            return _context.Reviewers.Any(r => r.Id == reviewerId);
        }

        public bool ReviewerExists(string firstName, string lastName)
        {
            return _context.Reviewers.Any(r => (r.FirstName.Trim().ToUpper() == firstName.Trim().ToUpper()) &&
            (r.LastName.Trim().ToUpper() == lastName.Trim().ToUpper()));
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _context.Reviewers.Update(reviewer);
            return Save();
        }
    }
}
