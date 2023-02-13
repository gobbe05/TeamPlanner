using TeamPlanner.Models;
using TeamPlanner.ViewModels;

namespace TeamPlanner.Interfaces
{
    public interface IGroupRepository
    {
        bool Create(Group group);
        bool CreateLink(GroupUser groupUser);
        Group GetById(int id);
        Group GetByName(string name);
        Task<List<AccountSummary>> GetAllUsersByGroup(int groupId);
        Task<List<AccountSummary>> GetAllUsersByGroup(int groupId, string week);
        Task<IEnumerable<Group>> GetAllGroupsByUser(string uid);    
        List<Group> GetAll();
        bool Delete(Group group);
        bool Update(Group group);
        bool Save();
    }
}
