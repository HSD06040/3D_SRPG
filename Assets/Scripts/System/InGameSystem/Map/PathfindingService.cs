using System.Collections.Generic;
using UnityEngine;

public class PathfindingService
{
    readonly MapSystem mapSystem;

    public PathfindingService(MapSystem mapSystem)
    {
        this.mapSystem = mapSystem; 
    }

    public List<Tile> CalculateMoveableTiles(BaseUnit unit)
    {
        Vector2Int unitPos = unit.CurPos;
        
        if (!mapSystem.TryGetTile(unitPos, out Tile startTile))
        {
            Debug.LogError("유닛의 현재 위치에 타일이 없습니다.");
            return new List<Tile>();
        }

        int movement = unit.UnitData.StatData.Movement;
        List<Tile> moveableTiles = new List<Tile>();

        Queue<(Tile tile, int distance)> queue = new();
        Dictionary<Tile, int> visited = new();

        queue.Enqueue((startTile, 0));
        visited.Add(startTile, 0);

        while (queue.Count > 0)
        {
            var (currentTile, currentDist) = queue.Dequeue();

            if (currentDist >= movement) continue;

            Vector2Int curPos = currentTile.Pos;
            Vector2Int[] neighbors = new Vector2Int[]
            {
                curPos + Vector2Int.up,
                curPos + Vector2Int.down,
                curPos + Vector2Int.left,
                curPos + Vector2Int.right,
            };

            foreach (Vector2Int neighborPos in neighbors)
            {
                if (mapSystem.TryGetTile(neighborPos, out Tile neighborTile))
                {
                    if (visited.ContainsKey(neighborTile) || !CanMoveTo(neighborTile, unit))
                    {
                        continue;
                    }

                    int nextDist = currentDist + 1;

                    if (nextDist <= movement)
                    {
                        visited.Add(neighborTile, nextDist);
                        moveableTiles.Add(neighborTile);
                        queue.Enqueue((neighborTile, nextDist));
                    }
                }
            }
        }

        return moveableTiles;
    }
    
    public void OnUnitMove(BaseUnit unit, Tile toTile, List<Tile> movableTiles)
    {
        if (!mapSystem.TryGetTile(unit.movementManager.TargetPos, out Tile startTile))
        {
            Debug.LogError($"유닛 {unit.name}의 시작 타일을 찾을 수 없습니다: {unit.movementManager.TargetPos}");
            return;
        }

        List<Tile> path = FindPath(startTile, toTile, unit, movableTiles);

        if (path == null || path.Count == 0)
        {
            Debug.Log("경로를 찾을 수 없거나 이미 해당 위치입니다.");
            return;
        }

        List<Vector3> positions = new List<Vector3>();
        List<Vector3> directions = new List<Vector3>();

        Vector3 currentWorldPos = unit.transform.position;

        foreach (Tile tile in path)
        {
            Vector3 targetWorldPos = tile.transform.position;

            Vector3 dir = (targetWorldPos - currentWorldPos).normalized;

            positions.Add(targetWorldPos);
            directions.Add(dir);

            currentWorldPos = targetWorldPos;
        }

        PathFindData pathData = new PathFindData(positions, directions);

        unit.movementManager.MoveUnit(pathData);
        unit.movementManager.SetTurnPosition(toTile.Pos);
    }

#region A* Pathfinding
    private List<Tile> FindPath(Tile startNode, Tile targetNode, BaseUnit unit, List<Tile> movableTiles)
    {
        // f = g + h;
        // -h = g - f;
        // h = -g + f;
        // h = f - g;   계산으로 h 비교  
        int maxMovement = unit.UnitData.StatData.Movement;
        HashSet<Tile> movableSet = new HashSet<Tile>(movableTiles);
        List<Tile> openSet = new List<Tile>(); // 방문할 노드 검사용
        HashSet<Tile> closedSet = new HashSet<Tile>(); // 방문한 노드 검사용

        Dictionary<Tile, Tile> parents = new Dictionary<Tile, Tile>(); // 최적 경로 추적용 노드 맵 (트리)
        Dictionary<Tile, int> gCost = new Dictionary<Tile, int>(); // 시작 노드부터 현재 노드까지의 비용 맵
        Dictionary<Tile, int> fCost = new Dictionary<Tile, int>(); // 시작 노드부터 목표 노드까지의 예상 비용 맵

        openSet.Add(startNode); // 처음부터 시작
        gCost[startNode] = 0;
        fCost[startNode] = GetDistance(startNode, targetNode);

        while (openSet.Count > 0)
        {
            // F cost가 가장 낮은 노드 찾기
            Tile current = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (fCost.ContainsKey(openSet[i]) && fCost.ContainsKey(current)) // Null 체크
                {
                    if (fCost[openSet[i]] < fCost[current] ||
                       (fCost[openSet[i]] == fCost[current] && gCost.ContainsKey(openSet[i]) && gCost.ContainsKey(current) &&
                        (fCost[openSet[i]] - gCost[openSet[i]]) < (fCost[current] - gCost[current]))) // H cost 비교
                    {
                        current = openSet[i];
                    }
                }
            }

            if (current == targetNode) // 목표 도착
            {
                return RetracePath(startNode, targetNode, parents); // 경로 재구성
            }

            // 도착하지 못함

            openSet.Remove(current); // 현재 노드를 열린 집합에서 제거
            closedSet.Add(current); // 현재 노드를 닫힌 집합에 추가

            foreach (Tile neighbor in GetNeighbors(current)) // 이웃 노드 검사
            {
                if (!movableSet.Contains(neighbor) || closedSet.Contains(neighbor) || !CanMoveTo(neighbor, unit))
                {
                    continue;
                }

                int newMovementCostToNeighbor = gCost[current] + 1; // 이동 비용 계산 (모든 이동 비용이 1로 동일)

                if (newMovementCostToNeighbor > maxMovement)
                {
                    continue;
                }

                if (!openSet.Contains(neighbor) || newMovementCostToNeighbor < gCost.GetValueOrDefault(neighbor, int.MaxValue)) // 새로운 경로가 더 나음
                {
                    gCost[neighbor] = newMovementCostToNeighbor; // 인접 타일 코스트 지정
                    int h = GetDistance(neighbor, targetNode); // H cost 계산
                    fCost[neighbor] = newMovementCostToNeighbor + h; // F cost 업데이트
                    parents[neighbor] = current; // 부모 노드 설정

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor); // 열린 집합에 추가
                    }
                }
            }

            // 다시 반복
        }

        return null; // 경로 없음
    }

    private List<Tile> RetracePath(Tile startNode, Tile endNode, Dictionary<Tile, Tile> parents)
    {
        List<Tile> path = new List<Tile>();
        Tile currentStep = endNode;

        while (currentStep != startNode)
        {
            path.Add(currentStep);
            if (parents.ContainsKey(currentStep))
            {
                currentStep = parents[currentStep];
            }
            else
            {
                break; // 예외 처리
            }
        }

        path.Reverse(); // 끝 - 시작 순서이므로 뒤집기
        return path;
    }

    // 맨해튼 거리 (그리드 이동 시 휴리스틱)
    private int GetDistance(Tile nodeA, Tile nodeB)
    {
        int dstX = Mathf.Abs(nodeA.Pos.x - nodeB.Pos.x);
        int dstY = Mathf.Abs(nodeA.Pos.y - nodeB.Pos.y);
        return dstX + dstY;
    }

    // 이웃 타일 가져오기 헬퍼 (중복 코드 제거용)
    private List<Tile> GetNeighbors(Tile currentTile)
    {
        List<Tile> neighbors = new List<Tile>();
        Vector2Int curPos = currentTile.Pos;
        Vector2Int[] offsets = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (var offset in offsets)
        {
            if (mapSystem.TryGetTile(curPos + offset, out Tile neighborTile))
            {
                neighbors.Add(neighborTile);
            }
        }
        return neighbors;
    }
    #endregion

    private bool CanMoveTo(Tile tile, BaseUnit unit)
    {
        if (tile.Data.isWall)
            return false;

        if (!tile.Data.isWakable)
        {
            return unit.UnitData.isFly;
        }

        return true;
    }
}