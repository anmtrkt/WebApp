using System.Dynamic;
using System.Reflection.Metadata;

namespace WebApp.Web
{
    /// <summary>
    /// Пути для эндпоинтов
    /// </summary>
    public class Routes
    {
        private const string StartRouteSegment = "api/v1";

        #region Auth

        private const string AuthRouteSegment = "auth";
        private const string AuthRoute = $"{StartRouteSegment}/{AuthRouteSegment}";

        public const string LoginRoute = $"{AuthRoute}/login";
        public const string RegistrationRoute = $"{AuthRoute}/registration";
        public const string RefreshRoute = $"{AuthRoute}/refresh";

        #endregion

        #region AdminPanel
        private const string AdminRouteSegment = "adm";
        private const string AdminRoute = $"{StartRouteSegment}/{AdminRouteSegment}";

        public const string RoleRoute = $"{AdminRoute}/{{UserId}}/rolescontrol";
        public const string AllUsersRoute = $"{AdminRoute}/user/all";
        public const string OneUserByIdRoute = $"{AdminRoute}/user/id={{UserId}}";
        public const string OneUserByEmailRoute = $"{AdminRoute}/user/mail={{Email}}";
        public const string DeleteUserRoute = $"{AdminRoute}/user/{{UserId}}/Delete";

        #endregion

        #region Booking

        private const string BookRouteSegment = "booking";
        private const string BookRoute = $"{StartRouteSegment}/{BookRouteSegment}";

        public const string AllUserBookingsRoute = $"{BookRoute}/all";
        public const string NewBookingRoute = $"{BookRoute}/new";
        public const string DeleteBookingRoute = $"{BookRoute}/{{BookingId}}/delete";
        public const string OneBookingRoute = $"{BookRoute}/{{BookingId}}";


        #endregion

        #region Hotel

        private const string HotelRouteSegment = "hotel";
        private const string HotelRoute = $"{StartRouteSegment}/{HotelRouteSegment}";

        public const string AllHotelsRoute = $"{HotelRoute}/all";
        public const string OneHotelRoute = $"{HotelRoute}/{{HotelId}}/info";
        public const string AllHotelRoomsRoute = $"{HotelRoute}/{{HotelId}}/rooms";
        public const string CreateHotelRoute = $"{HotelRoute}/new";
        public const string DeleteHotelRoute = $"{HotelRoute}/delete";
        public const string FindByLocationRoute = $"{HotelRoute}/{{Location}}";

        #endregion

        #region Room

        private const string RoomRouteSegment = "room";
        private const string RoomRoute = $"{StartRouteSegment}/{RoomRouteSegment}";

        public const string AllRoomsRoute = $"{RoomRoute}/all";
        public const string CreateRoomRoute = $"{RoomRoute}/new";
        public const string DeleteRoomRoute = $"{RoomRoute}/delete";
        public const string FindRoomRoute = $"{RoomRoute}/serv={{Services}}&&{{MinPrice}}&&{{MaxPrice}}";
        public const string JustRoomRoute = $"{RoomRoute}/{{RoomId}}";

        #endregion

        #region Upload
        private const string UploadRouteSegment = "upload";
        private const string UploadRoute = $"{StartRouteSegment}/{UploadRouteSegment}";

        public const string UploadHotelPhotoRoute = $"{UploadRoute}/hotel/photo";
        #endregion
    }
}
