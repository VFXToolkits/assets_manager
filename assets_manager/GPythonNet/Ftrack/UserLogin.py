import Base

import ftrack_api


class UserLogin(Base):

	def user_login(self, api_user: str, api_key: str) -> ftrack_api.Session:
		session=ftrack_api.Session(
		server_url=self.server_url,
		api_user=api_user,
		api_key=api_key
		)
		return session


	def get_all_type() -> list:
		return list(self.server_connect_api.types.keys())


	def get_user_info() -> dict:
		pass
