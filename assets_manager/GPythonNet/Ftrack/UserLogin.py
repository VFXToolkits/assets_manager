from AMBase import Base

import ftrack_api


class UserLogin(Base):

	def __init__(self, api_user: str, api_key: str):
		pass
		# session=ftrack_api.Session(
		# server_url=self.server_url,
		# api_user=api_user,
		# api_key=api_key
		# )
		# self.server_connect_api = session

	def get_all_type(self) -> list:
		return list(self.server_connect_api.types.keys())

	def get_user_info(self) -> list:
		return ["zuokangbo"]

