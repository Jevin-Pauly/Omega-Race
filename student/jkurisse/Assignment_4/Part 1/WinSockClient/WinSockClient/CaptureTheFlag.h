// CaptureTheFlag
// AB 4/21

#ifndef _CaptureTheFlag
#define _CaptureTheFlag

#include "Position.h"
#include "CTFPlayer.h"
#include "CTFField.h"

class CaptureTheFlag
{
public:
	enum class Direction { Hor, Vert };

	CaptureTheFlag();
	~CaptureTheFlag() = default;
	CaptureTheFlag(const CaptureTheFlag&) = delete;
	CaptureTheFlag& operator=(const CaptureTheFlag&) = delete;

	void MoveP1(Direction d, int num);
	void MoveP2(Direction d, int num);
	Position GetP1Pos() { return P1.GetPos(); }
	Position GetP2Pos() { return P2.GetPos(); }
	void SetP1Pos(Position p) { Field.ClearCharAtPos(P1.GetPos()); P1.SetPos(p); }
	void SetP2Pos(Position p) { Field.ClearCharAtPos(P2.GetPos()); P2.SetPos(p); }
	void UpdateField();
	char TestForWinner();
	void Display();

private:
	CTFField Field;
	CTFPlayer P1;
	CTFPlayer P2;

	void MovePlayer(CTFPlayer& P, Direction d, int num);
};


#endif _CaptureTheFlag

