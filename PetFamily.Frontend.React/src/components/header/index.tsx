import { useNavigate } from "react-router-dom";
import account from "../../assets/account.png";
import favorite from "../../assets/favorite.png";
import logo from "../../assets/logo.png";
import logout from "../../assets/logout.png";
import registration from "../../assets/registration.png";

var isLogined = true;
var isUserVolunteer = false;

export const Header = () => {
	const navigate = useNavigate();

	const handleLogoClick = () => {
		navigate("/");
	};

	const handleProfileClick = () => {
		if (!isLogined) {
			navigate("/login");
		} else {
			const userId = 1;
			const volunteerId = 1;
			const profilePath = isUserVolunteer
				? `/volunteers/${volunteerId}`
				: `/users/${userId}`;
			navigate(profilePath);
		}
	};

	return (
		<header className="bg-orange-300 px-3">
			<div className="flex justify-between items-center p-4">
				<div className="flex items-center">
					<img
						src={logo}
						alt="Логотип"
						className="h-9 w-9 filter invert cursor-pointer"
						onClick={handleLogoClick}
					/>
				</div>
				<div className="text-3xl text-white font-bold">PetFamily</div>

				<div className="flex items-center space-x-2">
					{!isUserVolunteer && (
						<div className="text-white font-bold cursor-pointer">
							<img
								src={favorite}
								alt="Избранное"
								className="h-7 w-7 filter invert"
								onClick={() => navigate("/favorites")}
							/>
						</div>
					)}
					{!isLogined ? (
						<>
							<img
								src={account}
								alt="Аккаунт"
								className="h-7 w-7 mx-3 filter invert cursor-pointer"
								onClick={() => navigate("/login")}
							/>
							<img
								src={logout}
								alt="Выход"
								className="h-7 w-7 filter invert cursor-pointer"
								onClick={() => {
									// Выход из аккаунта
								}}
							/>
						</>
					) : (
						<img
							src={registration}
							alt="Регистрация"
							className="h-7 w-7 filter invert cursor-pointer"
							onClick={handleProfileClick}
						/>
					)}
				</div>
			</div>
		</header>
	);
};
