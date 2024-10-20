import github from "../../assets/github-account.png";

export const Footer = () => {
	return (
		<footer className="bg-orange-300 mt-5 flex items-center justify-between p-3 text-2xl">
			<div>
				<a href="https://github.com/haxnted" className="bg-white">
					<img src={github} alt="github" className="h-7 w-7" />
				</a>
			</div>
			<div className="text-white text-lg">@ 2024. Dev: Haxnted</div>
		</footer>
	);
};
