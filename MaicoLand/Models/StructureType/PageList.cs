using System;
using System.Collections.Generic;
using System.Linq;
using MaicoLand.Models.Entities;

namespace MaicoLand.Models
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set;  }
        public int TotalPages{ get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        
        public PagedList(List<T> items , int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        public static PagedList<String> ToNewsPagedList(IQueryable<News> source , int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source
                .Skip((pageNumber - 1) * pageSize).OrderByDescending(a=> a.CreatedDate)
                           
                         .Take(pageSize)
                         .Select((a)=> a.Id)
                         .ToList();
            return new PagedList<String>(items, count, pageNumber, pageSize);
        }
        public static PagedList<String> ToLandPagedList(IQueryable<LandPlanning> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source
                .Skip((pageNumber - 1) * pageSize).OrderByDescending(a => a.CreatedDate)

                         .Take(pageSize)
                            .Select((a) => a.Id)
                         .ToList();
            return new PagedList<String>(items, count, pageNumber, pageSize);
        }
        public static PagedList<String> ToSalePostPagedList(IQueryable<SalePost> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source
                .Skip((pageNumber - 1) * pageSize).OrderByDescending(a => a.CreatedDate)

                         .Take(pageSize)
                            .Select((a) => a.Id)
                         .ToList();
            return new PagedList<String>(items, count, pageNumber, pageSize);
        }

    }
}
//<<<<<<< hoa
//return BlocBuilder<NewsAddBloc, NewsAddState>(
//  builder: (context, state) {
//    print("_NewsAddButton" + state.status.toString());
//    return SizedBox(
//      width: double.infinity,
//      child: ElevatedButton(
//        key: const Key('NewsAddForm_continue_raisedButton'),
//            child: const Text('Lưu', style: TextStyle(color: Colors.white)),
//            onPressed: state.status.isValidated
//                ? () async {
//        try
//        {
//            context.read<NewsAddBloc>().add(NewsAddSubmitted(type));
//            Navigator.of(context)
//                .pushNamedAndRemoveUntil("/", (route) => false);

//            ScaffoldMessenger.of(context).showSnackBar(
//              SnackBar(
//                content: const Text("Đăng bài thành công"),
//                          backgroundColor:
//            Theme.of(context).colorScheme.primary,
//                        ),
//                      );
//        }
//        catch (e)
//        {
//            ScaffoldMessenger.of(context).showSnackBar(
//                        const SnackBar(
//                          content: Text(
//                              "Lỗi đăng bài. Vui lòng khởi động lại phần mềm!"),
//                          backgroundColor: Colors.red,
//                        ),
//                      );
//            print(e.toString());
//        }
//        context.read<NewsAddBloc>().add(NewsAddInitial());
//    }
//                : null,
//          ),
//        );
//=======